using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace P2PChat
{
    public partial class Form1 : Form
    {
        // 客戶端相關變數
        private System.Windows.Forms.Timer connectionTimer;
        private int remainingSeconds = 30;
        private TcpClient tcpClient;
        private Form progressForm;

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            btnEnableTCP.Click += btnEnableTCP_Click;
            this.VisibleChanged += Form1_VisibleChanged;
        }
        // 初始化計時器
        private void InitializeTimer()
        {
            connectionTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000,
                Enabled = false
            };
            connectionTimer.Tick += ConnectionTimer_Tick;
        }
        // 表單載入時初始化
        private void Form1_Load(object sender, EventArgs e)
        {
            btnEnableTCP.Enabled = true;
            txtClientIP.Text = "";
            txtClientPORT.Text = "";
        }
        // 處理表單可見性改變事件
        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                btnEnableTCP.Enabled = true;
            }
        }
        // 顯示連接進度視窗
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

            progressForm.Controls.Add(new Label
            {
                Text = $"正在連接伺服器...\n剩餘時間：{remainingSeconds}秒",
                AutoSize = true,
                Location = new Point(20, 20)
            });

            progressForm.Show();
        }

        // 更新進度顯示內容
        private void UpdateProgressForm()
        {
            if (progressForm?.IsDisposed == false)
            {
                ((Label)progressForm.Controls[0]).Text = $"正在連接伺服器...\n剩餘時間：{remainingSeconds}秒";
            }
        }

        // 處理連接按鈕點擊事件
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

                // 嘗試連接伺服器
                tcpClient = new TcpClient();
                var connectTask = tcpClient.ConnectAsync(txtClientIP.Text, int.Parse(txtClientPORT.Text));

                // 等待連接完成
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    try
                    {
                        await Task.WhenAny(connectTask, Task.Delay(30000, cts.Token));

                        if (!connectTask.IsCompleted)
                        {
                            throw new TimeoutException("連接超時");
                        }

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
                // 連接成功處理
                connectionTimer.Stop();
                progressForm?.Close();
                MessageBox.Show("連線已接受", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 開啟聊天視窗
                var chatForm = new ChatForm(tcpClient, this);
                chatForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                HandleConnectionError(ex);
            }
        }
        // 處理連接錯誤
        private void HandleConnectionError(Exception ex)
        {
            connectionTimer.Stop();
            btnEnableTCP.Enabled = true;
            progressForm?.Close();
            MessageBox.Show($"連接失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        // 處理等待計時器事件
        private void ConnectionTimer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;
            UpdateProgressForm();

            if (remainingSeconds <= 0)
            {
                connectionTimer.Stop();
                btnEnableTCP.Enabled = true;
                tcpClient?.Close();
                tcpClient = null;

                if (progressForm?.IsDisposed == false)
                {
                    var lblProgress = (Label)progressForm.Controls[0];
                    lblProgress.Text = "伺服器無回應";
                    lblProgress.ForeColor = Color.Red;
                }
            }
        }
        // 關閉客戶端視窗
        private void btnDisableTCP_Click(object sender, EventArgs e) => Close();
    }
}