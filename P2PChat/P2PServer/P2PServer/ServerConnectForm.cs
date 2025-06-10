using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using P2PChat;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace P2PServer
{
    public partial class Form1 : Form
    {
        private TcpListener tcpListener;
        private System.Windows.Forms.Timer connectionTimer;
        private Form progressForm;
        private bool isListenerActive = false;
        private List<ChatForm> activeChatForms = new List<ChatForm>();
        private readonly object _lock = new object(); 

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            btnEnableServerTCP.Click += btnEnableServerTCP_Click;
            this.FormClosing += Form1_FormClosing; 
        }

        // 初始化等待連接計時器
        private void InitializeTimer()
        {
            connectionTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000,
                Enabled = false
            };
        }

        // 初始化伺服器設定
        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateUIState(false);
            txtServerIP.Text = GetLocalIPAddress() ?? "127.0.0.1";
            txtServerPORT.Text = "8888";
        }

        // 伺服器啟動/停止按鈕事件
        private async void btnEnableServerTCP_Click(object sender, EventArgs e)
        {
            if (!isListenerActive) 
            {
                if (string.IsNullOrEmpty(txtServerIP.Text) || string.IsNullOrEmpty(txtServerPORT.Text))
                {
                    MessageBox.Show("請輸入IP地址和Port", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    int port = int.Parse(txtServerPORT.Text);
                    tcpListener = new TcpListener(IPAddress.Parse(txtServerIP.Text), port);
                    tcpListener.Start();
                    UpdateUIState(true);

                    ShowProgressForm();
                    connectionTimer.Start();

                    await ListenForClientsAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"伺服器啟動失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CleanupListener();
                    UpdateUIState(false);
                }
            }
            else 
            {
                CleanupListener();
                UpdateUIState(false);
                MessageBox.Show("伺服器已停止監聽新連線。");

                if (activeChatForms.Count > 0)
                {
                    DialogResult result = MessageBox.Show(
                        $"是否要中斷所有 {activeChatForms.Count} 個正在進行的連線？",
                        "中斷連線確認",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        CloseAllChatForms();
                    }
                }
            }
        }

        // 非同步監聽客戶端連線
        private async Task ListenForClientsAsync()
        {
            try
            {
                while (isListenerActive)
                {
                    TcpClient newClient = await tcpListener.AcceptTcpClientAsync();
                    if (connectionTimer.Enabled)
                    {
                        connectionTimer.Stop();
                        progressForm?.Close();
                    }
                    this.Invoke((MethodInvoker)delegate { HandleNewClient(newClient); });
                }
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.Interrupted)
            {
                Console.WriteLine("TcpListener stopped successfully.");
            }
            catch (Exception ex)
            {
                if (isListenerActive)
                {
                    MessageBox.Show($"監聽錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Invoke((MethodInvoker)delegate {
                        CleanupListener();
                        UpdateUIState(false);
                    });
                }
            }
        }

        // 處理新客戶端連線
        private async void HandleNewClient(TcpClient newClient)
        {
            string clientIP = ((IPEndPoint)newClient.Client.RemoteEndPoint).Address.ToString();
            DialogResult result = MessageBox.Show($"是否接受來自 {clientIP} 的連接？", "接受連接", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    byte[] acceptSignal = Encoding.Unicode.GetBytes("<ACCEPT_CHAT>");
                    await newClient.GetStream().WriteAsync(acceptSignal, 0, acceptSignal.Length);

                    MessageBox.Show($"與 {clientIP} 的連線已確認", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ChatForm chatSession = new ChatForm(newClient, this);
                    chatSession.FormClosed += ChatForm_FormClosed;

                    lock (_lock)
                    {
                        activeChatForms.Add(chatSession);
                    }
                    chatSession.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"處理新連線時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    newClient.Close();
                }
            }
            else
            {
                newClient.Close();
            }
        }

        // 關閉程式 按鈕
        private void btndisbleServerTCP_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        // 子聊天視窗關閉
        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is ChatForm closedForm)
            {
                lock (_lock)
                {
                    activeChatForms.Remove(closedForm);
                }
            }
        }

        // 主視窗關閉 (處理'X'按鈕和Close())
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (activeChatForms.Count > 0 || isListenerActive)
            {
                DialogResult result = MessageBox.Show("確定要關閉伺服器嗎？\n所有正在進行的連線將會中斷。",
                                                     "關閉確認",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    e.Cancel = true; 
                }
                else
                {
                    CleanupListener(); 
                    CloseAllChatForms(); 
                }
            }
        }

        private string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch { /* 忽略網路錯誤 */ }
            return null;
        }

        private void ShowProgressForm()
        {
            progressForm?.Close();
            progressForm?.Dispose();
            progressForm = new Form
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                Text = "等待連接",
                ControlBox = false
            };
            progressForm.Controls.Add(new Label
            {
                Text = $"正在等待客戶端連線...",
                AutoSize = true,
                Location = new Point(20, 40)
            });
            progressForm.Show(this);
        }

        private void UpdateUIState(bool listening)
        {
            isListenerActive = listening;
            btnEnableServerTCP.Text = listening ? "停止監聽" : "開始監聽";
            txtServerIP.Enabled = !listening;
            txtServerPORT.Enabled = !listening;
        }

        private void CleanupListener()
        {
            if (isListenerActive)
            {
                isListenerActive = false;
                tcpListener?.Stop();
            }
            connectionTimer?.Stop();
            progressForm?.Close();
        }

        private void CloseAllChatForms()
        {
            List<ChatForm> formsToClose;
            lock (_lock)
            {
                formsToClose = new List<ChatForm>(activeChatForms);
            }

            foreach (var chatForm in formsToClose)
            {
                chatForm.ForceClose();
            }

            lock (_lock)
            {
                activeChatForms.Clear();
            }
        }
    }
}