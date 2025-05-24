using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using P2PChat; // 加入這個 using 指令，以便引用 P2PChat 命名空間下的 ChatForm

namespace P2PServer
{
    public partial class Form1 : Form
    {
        // 伺服器相關變數
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

        // 初始化計時器設定
        private void InitializeTimer()
        {
            connectionTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000,
                Enabled = false
            };
            connectionTimer.Tick += ConnectionTimer_Tick;
        }

        // 表單載入時的初始化設定
        private void Form1_Load(object sender, EventArgs e)
        {
            btnEnableServerTCP.Enabled = true;
            btnEnableServerTCP.Text = "開始監聽";
            txtServerIP.Text = "127.0.0.1"; // 預設本地IP
            txtServerPORT.Text = "8888"; // 預設Port
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

        // 更新進度視窗的顯示內容
        private void UpdateProgressForm()
        {
            if (progressForm?.IsDisposed == false)
            {
                ((Label)progressForm.Controls[0]).Text = $"等待客戶端連接...\n剩餘時間：{remainingSeconds}秒";
            }
        }

        // 處理伺服器啟動/停止按鈕點擊事件
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
                    // 啟動伺服器監聽
                    int port = int.Parse(txtServerPORT.Text);
                    tcpListener = new TcpListener(IPAddress.Any, port); // 使用Any來監聽所有網路介面
                    tcpListener.Start();
                    isServerRunning = true;
                    btnEnableServerTCP.Text = "停止監聽";

                    // 啟動等待計時
                    remainingSeconds = 30;
                    connectionTimer.Start();
                    ShowProgressForm();

                    await ListenForClientsAsync();
                }
                else
                {
                    // 停止伺服器監聽
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
                    TcpClient client = await tcpListener.AcceptTcpClientAsync();

                    // 客戶端連接成功處理
                    connectionTimer.Stop();
                    progressForm?.Close();
                    MessageBox.Show("客戶端已連接", "連接成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 開啟聊天視窗
                    var serverChatForm = new ChatForm(client, this);
                    serverChatForm.Show();
                    this.Hide(); // 隱藏伺服器監聽視窗

                    // TODO: 在此處處理客戶端與伺服器之間的通訊
                    // 目前只是接受連線，實際的聊天邏輯還需要在此處或新的方法中實現
                    // break; // 如果需要支援多個客戶端需要修改此處，這裡為了簡單演示單客戶端連線，先保留 break
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

        // 關閉伺服器視窗
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