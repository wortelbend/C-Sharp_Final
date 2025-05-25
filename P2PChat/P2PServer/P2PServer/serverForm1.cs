using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using P2PChat;

namespace P2PServer
{
    // 伺服器主表單，處理TCP連接和客戶端監聽
    public partial class Form1 : Form
    {
        // TCP伺服器相關變數
        private TcpListener tcpListener;
        private System.Windows.Forms.Timer connectionTimer;
        private int remainingSeconds = 30;
        private Form progressForm;
        public bool isServerRunning = false;

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

        // 處理伺服器啟動/停止
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

        // 停止伺服器並重置所有狀態
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
                    TcpClient client = await tcpListener.AcceptTcpClientAsync();

                    connectionTimer.Stop();
                    progressForm?.Close();
                    MessageBox.Show("客戶端已連接", "連接成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var serverChatForm = new ChatForm(client, this);
                    serverChatForm.Show();
                    this.Hide();
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
    }
}