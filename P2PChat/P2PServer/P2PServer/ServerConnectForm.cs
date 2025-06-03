using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using P2PChat; // Assuming this is your ChatForm namespace
using System.Text;
using System.Linq;
using System.Collections.Generic; // Needed for List<T>

namespace P2PServer
{
    public partial class Form1 : Form
    {
        // TCP伺服器相關變數
        private TcpListener tcpListener;
        private System.Windows.Forms.Timer connectionTimer;
        private int remainingSeconds = 30;
        private Form progressForm;
        public bool isServerRunning = false;
        // private ChatForm activeChatForm; // OLD: For single chat
        private List<ChatForm> activeChatForms = new List<ChatForm>(); // NEW: For multiple chats

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            btnEnableServerTCP.Click += btnEnableServerTCP_Click;
            // Ensure your designer has a button named btndisbleServerTCP (or rename the event handler)
            // Example: this.btndisbleServerTCP.Click += new System.EventHandler(this.btndisbleServerTCP_Click_1);
        }

        // 初始化等待連接計時器
        private void InitializeTimer()
        {
            connectionTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000,
                Enabled = false
            };
            connectionTimer.Tick += ConnectionTimer_Tick;
        }

        // 初始化伺服器設定
        private void Form1_Load(object sender, EventArgs e)
        {
            btnEnableServerTCP.Enabled = true;
            btnEnableServerTCP.Text = "開始監聽";
            string localIP = GetLocalIPAddress();
            txtServerIP.Text = !string.IsNullOrEmpty(localIP) ? localIP : "127.0.0.1";
            txtServerPORT.Text = "8888";
        }

        /// <summary>
        /// 取得本機的 IPv4 位址。
        /// </summary>
        /// <returns>本機的 IPv4 位址字串，如果沒有找到則回傳 null。</returns>
        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return null;
        }

        // 顯示等待連接的進度視窗
        private void ShowProgressForm()
        {
            // Close existing progress form if any
            progressForm?.Close();
            progressForm?.Dispose();

            progressForm = new Form
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                Text = "等待連接"
            };

            progressForm.Controls.Add(new Label
            {
                Name = "lblProgressStatus", // Give it a name for easier access
                Text = $"等待客戶端連接...\n剩餘時間：{remainingSeconds}秒",
                AutoSize = true,
                Location = new Point(20, 20)
            });

            progressForm.Show(this); // Show as a non-modal dialog owned by Form1
        }

        // 更新等待連接進度視窗
        private void UpdateProgressForm()
        {
            if (progressForm?.IsDisposed == false)
            {
                var lbl = progressForm.Controls.Find("lblProgressStatus", true).FirstOrDefault() as Label;
                if (lbl != null)
                {
                    lbl.Text = $"等待客戶端連接...\n剩餘時間：{remainingSeconds}秒";
                }
            }
        }

        // 伺服器啟動/停止
        private async void btnEnableServerTCP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtServerIP.Text) || string.IsNullOrEmpty(txtServerPORT.Text))
            {
                MessageBox.Show("請輸入IP地址和Port", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (!isServerRunning)
                {
                    int port = int.Parse(txtServerPORT.Text);
                    tcpListener = new TcpListener(IPAddress.Parse(txtServerIP.Text), port);
                    tcpListener.Start();
                    isServerRunning = true;
                    btnEnableServerTCP.Text = "停止監聽";
                    txtServerIP.Enabled = false; // Disable editing while server is running
                    txtServerPORT.Enabled = false;

                    remainingSeconds = 30; // Reset timer for the first connection attempt
                    ShowProgressForm(); // Show progress form
                    connectionTimer.Start(); // Start timer only when starting server

                    await ListenForClientsAsync();
                }
                else
                {
                    StopServer();
                    txtServerIP.Enabled = true; // Re-enable editing
                    txtServerPORT.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"伺服器操作失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StopServer(); // Ensure server is stopped on error
                txtServerIP.Enabled = true;
                txtServerPORT.Enabled = true;
            }
        }

        // 停止伺服器並重置狀態
        private void StopServer()
        {
            isServerRunning = false; // Set this first to stop the listening loop
            tcpListener?.Stop();

            // Close all active chat forms
            // Iterate over a copy of the list because closing a form will modify the original list
            List<ChatForm> formsToClose = new List<ChatForm>(activeChatForms);
            foreach (var chatForm in formsToClose)
            {
                chatForm.Close();
            }
            activeChatForms.Clear(); // Clear the list

            btnEnableServerTCP.Text = "開始監聽";
            // btnEnableServerTCP.Enabled = true; // This will be handled by ResetServerState or logic in btn_click
            connectionTimer.Stop();
            progressForm?.Close();
            progressForm?.Dispose(); // Dispose it properly

            // Re-enable UI elements if they were disabled
            txtServerIP.Enabled = true;
            txtServerPORT.Enabled = true;

            // this.Show(); // If you ever hide Form1, show it here.
        }

        // 非同步等待客戶端連接
        private async Task ListenForClientsAsync()
        {
            try
            {
                while (isServerRunning) // Loop to accept multiple clients
                {
                    TcpClient newClient = await tcpListener.AcceptTcpClientAsync(); // Wait for a new client

                    // At this point, a client is trying to connect.
                    // Stop the initial 30-second timer if it's still running,
                    // as we have received a connection attempt.
                    if (connectionTimer.Enabled)
                    {
                        connectionTimer.Stop();
                        progressForm?.Close(); // Close progress form as a connection is pending
                        progressForm?.Dispose();
                    }

                    // OLD LOGIC: Reject if already busy (for single client)
                    // if (activeChatForm != null && !activeChatForm.IsDisposed)
                    // {
                    //     MessageBox.Show("伺服器已忙碌，請稍後再試。", "連線被拒絕", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //     newClient.Close();
                    //     continue; // Continue listening for other clients
                    // }

                    string clientIP = ((IPEndPoint)newClient.Client.RemoteEndPoint).Address.ToString();
                    DialogResult result = DialogResult.None;

                    // Ensure MessageBox is shown on the UI thread
                    this.Invoke((MethodInvoker)delegate
                    {
                        // if (progressForm != null && !progressForm.IsDisposed) progressForm.Close(); // Already closed above or by timer
                        result = MessageBox.Show($"是否接受來自 {clientIP} 的連接？", "接受連接", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    });


                    if (result == DialogResult.Yes)
                    {
                        // No need to assign to a single TcpClient variable 'client' here if newClient is used directly
                        // connectionTimer.Stop(); // Already stopped when a client pended
                        // progressForm?.Close(); // Already closed

                        try
                        {
                            byte[] acceptSignal = Encoding.Unicode.GetBytes("<ACCEPT_CHAT>");
                            NetworkStream stream = newClient.GetStream();
                            await stream.WriteAsync(acceptSignal, 0, acceptSignal.Length);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"發送接受訊號失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            newClient.Close();
                            continue; // Continue to listen for other clients
                        }

                        // Create and show the chat form on the UI thread
                        this.Invoke((MethodInvoker)delegate
                        {
                            MessageBox.Show($"與 {clientIP} 的連線已確認", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ChatForm chatSession = new ChatForm(newClient, this); // Pass the new client
                            chatSession.FormClosed += ChatForm_FormClosed; // Use the updated event handler
                            activeChatForms.Add(chatSession);
                            chatSession.Show();
                        });


                        // *** KEY CHANGE: DO NOT STOP THE SERVER HERE ***
                        // StopServer(); // OLD: This stopped listening for more clients
                        // this.Hide(); // OLD: Don't hide the main form if you want to manage multiple sessions from it

                        // The server will continue listening due to the while(isServerRunning) loop
                    }
                    else
                    {
                        MessageBox.Show($"已拒絕來自 {clientIP} 的連接。", "連接被拒絕", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        newClient.Close();
                        // Continue listening for the next connection attempt
                    }
                }
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.Interrupted)
            {
                // This exception is expected when tcpListener.Stop() is called.
                // You can log this if needed, or just ignore it as part of normal shutdown.
                Console.WriteLine("TcpListener stopped successfully.");
            }
            catch (Exception ex)
            {
                // Only show error if server was supposed to be running
                if (isServerRunning)
                {
                    MessageBox.Show($"監聽錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Consider whether to call StopServer() here or let the user manually stop/restart
                    // For robustness, it might be good to stop it if a critical listening error occurs.
                    this.Invoke((MethodInvoker)delegate { StopServer(); });
                }
            }
        }

        // 處理等待計時器事件
        private void ConnectionTimer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;
            UpdateProgressForm();

            if (remainingSeconds <= 0)
            {
                connectionTimer.Stop();
                // Only stop the server if it's running AND no clients have connected yet.
                // If activeChatForms.Count is > 0, it means at least one client connected,
                // and the timer should have been stopped. This is a fallback.
                if (isServerRunning && activeChatForms.Count == 0)
                {
                    if (progressForm?.IsDisposed == false)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            var lbl = progressForm.Controls.Find("lblProgressStatus", true).FirstOrDefault() as Label;
                            if (lbl != null)
                            {
                                lbl.Text = "未收到連接請求。伺服器將停止。";
                                lbl.ForeColor = Color.Red;
                            }
                            // Give user a moment to see the message before closing progress and stopping server
                            Task.Delay(2000).ContinueWith(t =>
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    progressForm?.Close();
                                    progressForm?.Dispose();
                                    StopServer(); // Stop the server
                                });
                            });
                        });
                    }
                    else
                    {
                        StopServer(); // Stop the server
                    }
                    MessageBox.Show("等待超時，未收到任何客戶端連接請求。", "超時", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (isServerRunning && activeChatForms.Count > 0)
                {
                    // This case (timer ticks to 0 but clients are connected) ideally shouldn't happen
                    // as the timer should be stopped upon first client connection.
                    // If it does, just close the progress form if it's somehow still open.
                    progressForm?.Close();
                    progressForm?.Dispose();
                }
            }
        }

        // This button is likely intended to close the main server form
        private void btndisbleServerTCP_Click_1(object sender, EventArgs e) // Renamed from your example if it was different
        {
            StopServer(); // Ensure server resources are released
            Close(); // Close the main server form
        }

        // 重置伺服器狀態 (Called by ChatForm when it closes - might need adjustment)
        // This method's original purpose was to allow the server to listen again after a single chat.
        // With continuous listening, its role changes.
        public void ResetServerStateAfterChatClose() // Renamed for clarity
        {
            // This method might not be strictly needed if the server continuously listens.
            // However, if you want to update the UI or perform other actions when ALL chats are closed,
            // you could add logic here.
            // For now, the main thing is that ChatForm_FormClosed handles removing the form from the list.

            // If no more active chats and server was stopped for some reason (e.g. error),
            // then allow user to start listening again.
            if (activeChatForms.Count == 0 && !isServerRunning)
            {
                btnEnableServerTCP.Text = "開始監聽";
                btnEnableServerTCP.Enabled = true;
                txtServerIP.Enabled = true;
                txtServerPORT.Enabled = true;
            }
            // If server is still running and listening, button should remain "停止監聽"
        }

        // 當聊天視窗關閉時
        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChatForm closedForm = sender as ChatForm;
            if (closedForm != null)
            {
                activeChatForms.Remove(closedForm);
                // Optionally, update some status on the server form
                // For example, update a label showing "Active connections: X"
            }
            // Call the modified ResetServerState or new logic if needed
            ResetServerStateAfterChatClose();
        }
    }
}