using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace P2PServer
{
    public partial class Form1 : Form
    {
        private TcpListener tcpListener;
        private System.Windows.Forms.Timer connectionTimer;
        private int remainingSeconds = 30;
        private Form progressForm;
        private bool isServerRunning = false;

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            // 確保按鈕事件綁定
            btnEnableServerTCP.Click += new EventHandler(btnEnableServerTCP_Click);
        }

        private void InitializeTimer()
        {
            connectionTimer = new System.Windows.Forms.Timer();
            connectionTimer.Interval = 1000; // 1秒
            connectionTimer.Tick += ConnectionTimer_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnEnableServerTCP.Enabled = true;
            txtServerIP.Text = "127.0.0.1"; // 預設本地IP
            txtServerPORT.Text = "8888"; // 預設Port
        }

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

            Label lblProgress = new Label
            {
                Text = $"等待客戶端連接...\n剩餘時間：{remainingSeconds}秒",
                AutoSize = true,
                Location = new Point(20, 20)
            };

            progressForm.Controls.Add(lblProgress);
            progressForm.Show();
        }

        private void UpdateProgressForm()
        {
            if (progressForm != null && !progressForm.IsDisposed)
            {
                Label lblProgress = (Label)progressForm.Controls[0];
                lblProgress.Text = $"等待客戶端連接...\n剩餘時間：{remainingSeconds}秒";
            }
        }

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
                    // 開始監聽
                    int port = int.Parse(txtServerPORT.Text);
                    tcpListener = new TcpListener(IPAddress.Any, port); // 使用Any來監聽所有網路介面
                    tcpListener.Start();
                    isServerRunning = true;
                    btnEnableServerTCP.Text = "停止監聽";
                    btnEnableServerTCP.Enabled = true;

                    // 開始計時
                    remainingSeconds = 30;
                    connectionTimer.Start();
                    ShowProgressForm();

                    // 等待客戶端連接
                    await ListenForClientsAsync();
                }
                else
                {
                    // 停止監聽
                    tcpListener.Stop();
                    isServerRunning = false;
                    btnEnableServerTCP.Text = "開始監聽";
                    connectionTimer.Stop();
                    if (progressForm != null && !progressForm.IsDisposed)
                    {
                        progressForm.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"伺服器啟動失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isServerRunning = false;
                btnEnableServerTCP.Text = "開始監聽";
                btnEnableServerTCP.Enabled = true;
                if (progressForm != null && !progressForm.IsDisposed)
                {
                    progressForm.Close();
                }
            }
        }

        private async Task ListenForClientsAsync()
        {
            try
            {
                while (isServerRunning)
                {
                    TcpClient client = await tcpListener.AcceptTcpClientAsync();
                    // 當客戶端連接成功時
                    connectionTimer.Stop();
                    if (progressForm != null && !progressForm.IsDisposed)
                    {
                        progressForm.Close();
                    }
                    MessageBox.Show("客戶端已連接", "連接成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
            }
            catch (Exception ex)
            {
                if (isServerRunning)
                {
                    MessageBox.Show($"監聽錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isServerRunning = false;
                    btnEnableServerTCP.Text = "開始監聽";
                    btnEnableServerTCP.Enabled = true;
                    if (progressForm != null && !progressForm.IsDisposed)
                    {
                        progressForm.Close();
                    }
                }
            }
        }

        private void ConnectionTimer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;
            UpdateProgressForm();

            if (remainingSeconds <= 0)
            {
                connectionTimer.Stop();
                if (isServerRunning)
                {
                    tcpListener.Stop();
                    isServerRunning = false;
                    btnEnableServerTCP.Text = "開始監聽";
                    btnEnableServerTCP.Enabled = true;
                    if (progressForm != null && !progressForm.IsDisposed)
                    {
                        Label lblProgress = (Label)progressForm.Controls[0];
                        lblProgress.Text = "未收到連接請求";
                        lblProgress.ForeColor = Color.Red;
                    }
                }
            }
        }
    }
}