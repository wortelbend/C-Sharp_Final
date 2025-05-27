using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using P2PChat;
using System.Text;

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
        private ChatForm activeChatForm;

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            btnEnableServerTCP.Click += btnEnableServerTCP_Click;
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
            txtServerIP.Text = "127.0.0.1";
            txtServerPORT.Text = "8888";
        }

        // 顯示等待連接的進度視窗
        private void ShowProgressForm()
        {
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
                Text = $"等待客戶端連接...\n剩餘時間：{remainingSeconds}秒",
                AutoSize = true,
                Location = new Point(20, 20)
            });

            progressForm.Show();
        }

        // 更新等待連接進度視窗
        private void UpdateProgressForm()
        {
            if (progressForm?.IsDisposed == false)
            {
                ((Label)progressForm.Controls[0]).Text = $"等待客戶端連接...\n剩餘時間：{remainingSeconds}秒";
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
                    tcpListener = new TcpListener(IPAddress.Any, port);
                    tcpListener.Start();
                    isServerRunning = true;
                    btnEnableServerTCP.Text = "停止監聽";

                    remainingSeconds = 30;
                    connectionTimer.Start();
                    ShowProgressForm();

                    await ListenForClientsAsync();
                }
                else
                {
                    StopServer();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"伺服器啟動失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StopServer();
            }
        }

        // 停止伺服器並重置狀態
        private void StopServer()
        {
            tcpListener?.Stop();
            isServerRunning = false;
            btnEnableServerTCP.Text = "開始監聽";
            btnEnableServerTCP.Enabled = true;
            connectionTimer.Stop();
            progressForm?.Close();
        }

        // 非同步等待客戶端連接
        private async Task ListenForClientsAsync()
        {
            try
            {
                while (isServerRunning)
                {
                    TcpClient newClient = await tcpListener.AcceptTcpClientAsync();

                    if (activeChatForm != null && !activeChatForm.IsDisposed)
                    {
                        // 如新連線發現已有聊天視窗，通知並關閉新連線
                        MessageBox.Show("伺服器已忙碌，請稍後再試。", "連線被拒絕", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        newClient.Close(); // 關閉新的客戶端連線
                        continue;
                    }
                    // 獲取客戶端 IP 地址
                    string clientIP = ((IPEndPoint)newClient.Client.RemoteEndPoint).Address.ToString();
                    // 在 UI 執行緒上顯示確認訊息框
                    DialogResult result = (DialogResult)this.Invoke((Func<DialogResult>)(() =>
                    {
                        if (progressForm != null && !progressForm.IsDisposed) progressForm.Close();
                        return MessageBox.Show($"是否接受來自 {clientIP} 的連接？", "接受連接", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    }));

                    if (result == DialogResult.Yes)
                    {
                        TcpClient client = newClient;
                        connectionTimer.Stop();
                        progressForm?.Close();

                        try
                        {
                            // 向客戶端發送接受連線的確認訊號
                            byte[] acceptSignal = Encoding.Unicode.GetBytes("<ACCEPT_CHAT>");
                            NetworkStream stream = client.GetStream();
                            await stream.WriteAsync(acceptSignal, 0, acceptSignal.Length);
                        }
                        catch (Exception ex)
                        {
                            // 如果發送訊號失敗，關閉連線並處理錯誤
                            MessageBox.Show($"發送接受訊號失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            client.Close();
                            continue; 
                        }
                        // 顯示聊天視窗
                        activeChatForm = new ChatForm(client, this);
                        activeChatForm.FormClosed += ActiveChatForm_FormClosed;
                        activeChatForm.Show();
                        // 接受連線後停止監聽其他連線
                        StopServer();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show($"已拒絕來自 {clientIP} 的連接。", "連接被拒絕", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        newClient.Close();
                        // 會繼續監聽下一個連線請求
                    }
                }
            }
            catch (Exception ex)
            {
                if (isServerRunning)
                {
                    MessageBox.Show($"監聽錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    StopServer();
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
                if (isServerRunning)
                {
                    StopServer();
                    if (progressForm?.IsDisposed == false)
                    {
                        var lblProgress = (Label)progressForm.Controls[0];
                        lblProgress.Text = "未收到連接請求";
                        lblProgress.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void btndisbleServerTCP_Click(object sender, EventArgs e) => Close();
        // 重置伺服器狀態
        public void ResetServerState()
        {
            isServerRunning = false;
            btnEnableServerTCP.Text = "開始監聽";
            btnEnableServerTCP.Enabled = true;
        }
        //當活躍的聊天視窗關閉時
        //activeChatForm清除對已關閉視窗的引用
        private void ActiveChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            activeChatForm = null;
        }
    }
}