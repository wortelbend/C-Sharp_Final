using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;

namespace P2PChat
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer connectionTimer;
        private int remainingSeconds = 30;
        private TcpClient tcpClient;
        private Form progressForm;

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            // 確保按鈕事件綁定
            btnEnableTCP.Click += new EventHandler(btnEnableTCP_Click);
        }

        private void InitializeTimer()
        {
            connectionTimer = new System.Windows.Forms.Timer();
            connectionTimer.Interval = 1000; // 1秒
            connectionTimer.Tick += ConnectionTimer_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 表單載入時的初始化
            btnEnableTCP.Enabled = true;
            txtClientIP.Text = "";
            txtClientPORT.Text = "";
        }

        private void ShowProgressForm()
        {
            progressForm = new Form
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                Text = "連接中"
            };

            Label lblProgress = new Label
            {
                Text = $"正在連接伺服器...\n剩餘時間：{remainingSeconds}秒",
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
                lblProgress.Text = $"正在連接伺服器...\n剩餘時間：{remainingSeconds}秒";
            }
        }

        private async void btnEnableTCP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtClientIP.Text) || string.IsNullOrEmpty(txtClientPORT.Text))
            {
                MessageBox.Show("請輸入IP地址和Port", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // 開始連接計時
                remainingSeconds = 30;
                connectionTimer.Start();
                btnEnableTCP.Enabled = false;
                ShowProgressForm();

                // 嘗試連接
                tcpClient = new TcpClient();
                var connectTask = tcpClient.ConnectAsync(txtClientIP.Text, int.Parse(txtClientPORT.Text));

                // 等待連接完成或超時
                using (var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    try
                    {
                        await Task.WhenAny(connectTask, Task.Delay(30000, cts.Token));

                        if (!connectTask.IsCompleted)
                        {
                            throw new TimeoutException("連接超時");
                        }

                        // 檢查連接是否真的成功
                        if (!tcpClient.Connected)
                        {
                            throw new SocketException((int)SocketError.ConnectionRefused);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        throw new TimeoutException("連接超時");
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.ConnectionRefused)
                        {
                            throw new Exception("伺服器未啟動或拒絕連接");
                        }
                        throw;
                    }
                }

                // 如果成功連接
                connectionTimer.Stop();
                if (progressForm != null && !progressForm.IsDisposed)
                {
                    progressForm.Close();
                }
                MessageBox.Show("連線已接受", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 開啟聊天視窗，並將已連接的 tcpClient 傳入
                ChatForm chatForm = new ChatForm(tcpClient, this);
                chatForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                connectionTimer.Stop();
                btnEnableTCP.Enabled = true;
                if (progressForm != null && !progressForm.IsDisposed)
                {
                    progressForm.Close();
                }
                MessageBox.Show($"連接失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConnectionTimer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;
            UpdateProgressForm();

            if (remainingSeconds <= 0)
            {
                connectionTimer.Stop();
                btnEnableTCP.Enabled = true;

                if (tcpClient != null)
                {
                    tcpClient.Close();
                    tcpClient = null;
                }

                if (progressForm != null && !progressForm.IsDisposed)
                {
                    Label lblProgress = (Label)progressForm.Controls[0];
                    lblProgress.Text = "伺服器無回應";
                    lblProgress.ForeColor = Color.Red;
                }
            }
        }

        private void btnDisableTCP_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
