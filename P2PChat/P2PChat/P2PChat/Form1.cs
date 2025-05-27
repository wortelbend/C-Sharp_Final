using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Text;

namespace P2PChat
{
    public partial class Form1 : Form
    {
        // 客戶端連接相關變數
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

        // 初始化計時器，每秒觸發一次。
        private void InitializeTimer()
        {
            connectionTimer = new System.Windows.Forms.Timer { Interval = 1000, Enabled = false };
            connectionTimer.Tick += ConnectionTimer_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnEnableTCP.Enabled = true;
            txtClientIP.Text = "";
            txtClientPORT.Text = "";
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible) btnEnableTCP.Enabled = true;
        }

        // 顯示一個連接進度視窗，包含一個倒數計時器。
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

            // FormClosed 事件處理器：當進度視窗關閉時觸發
            progressForm.FormClosed += ProgressForm_FormClosed;

            progressForm.Show();
        }

        // 處理進度視窗關閉的事件。當進度視窗關閉時，會重新啟用連接按鈕並清除 progressForm 的引用。
        private void ProgressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 當進度視窗關閉時，重新啟用按鈕，並清理 progressForm 引用
            if (btnEnableTCP != null && !btnEnableTCP.IsDisposed) // 確保按鈕物件存在且未被 disposed
            {
                btnEnableTCP.Enabled = true;
            }
            progressForm = null;
        }

        // 更新視窗中顯示的倒數計時。
        private void UpdateProgressForm()
        {
            if (progressForm?.IsDisposed == false)
            {
                ((Label)progressForm.Controls[0]).Text = $"正在連接伺服器...\n剩餘時間：{remainingSeconds}秒";
            }
        }

        // 嘗試建立一個 TCP 連接，並在連接過程中顯示進度視窗和倒數計時。如果連接成功，它會開啟聊天視窗；如果失敗，則處理各種錯誤情況（如超時或連接被拒絕）。
        private async void btnEnableTCP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtClientIP.Text) || string.IsNullOrEmpty(txtClientPORT.Text))
            {
                MessageBox.Show("請輸入IP地址和Port", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                remainingSeconds = 30;
                connectionTimer.Start();
                btnEnableTCP.Enabled = false;
                ShowProgressForm();

                tcpClient = new TcpClient();
                var connectTask = tcpClient.ConnectAsync(txtClientIP.Text, int.Parse(txtClientPORT.Text));

                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    try
                    {
                        await Task.WhenAny(connectTask, Task.Delay(30000, cts.Token));
                        if (!connectTask.IsCompleted) throw new TimeoutException("連接超時");
                        if (!tcpClient.Connected) throw new SocketException((int)SocketError.ConnectionRefused);
                        // 連線成功後，等待伺服器的確認訊號
                        await WaitForServerConfirmation(tcpClient, cts.Token);

                    }
                    catch (OperationCanceledException) { throw new TimeoutException("連接超時"); }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.ConnectionRefused)
                            throw new SocketException((int)SocketError.ConnectionRefused);
                        throw;
                    }
                }

                // 收到伺服器確認後，顯示連線成功並開啟聊天視窗
                connectionTimer.Stop();
                progressForm?.Close();
                MessageBox.Show("連線已接受", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                new ChatForm(tcpClient, this).Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                // 檢查是否為連接被拒絕的特定錯誤
                if (ex is SocketException socketEx && socketEx.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    connectionTimer.Stop();
                    btnEnableTCP.Enabled = true;
                    progressForm?.Close();
                    // 顯示連線被拒絕訊息
                    MessageBox.Show("找無伺服器", "連線失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (ex is TimeoutException)
                {
                    connectionTimer.Stop();
                    btnEnableTCP.Enabled = true;
                    progressForm?.Close();
                    // 連接超時訊息
                    MessageBox.Show("連接超時，伺服器無回應", "連線失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    HandleConnectionError(ex);
                }
            }
        }

        // 等待伺服器的連線確認訊號。會從網路資料流中讀取資料，直到收到特定的確認訊號（例如 "<ACCEPT_CHAT>"）或超時。
        private async Task WaitForServerConfirmation(TcpClient client, CancellationToken cancellationToken)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[4096];
            StringBuilder receivedData = new StringBuilder();

            try
            {
                // 等待接收伺服器的確認訊號，例如 "<ACCEPT_CHAT>"
                using (var readCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token))
                {
                    while (!readCts.IsCancellationRequested)
                    {
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, readCts.Token);
                        if (bytesRead > 0)
                        {
                            receivedData.Append(Encoding.Unicode.GetString(buffer, 0, bytesRead));
                            // 檢查是否收到了接受訊號
                            if (receivedData.ToString().Contains("<ACCEPT_CHAT>"))
                            {
                                return; 
                            }
                        }
                        else if (bytesRead == 0)
                        {
                            // 連線已被遠端關閉
                            throw new SocketException((int)SocketError.ConnectionAborted);
                        }
                    }
                    // 迴圈結束是因取消，表示超時
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
                // 讀取超時或外部取消
                throw new TimeoutException("等待伺服器確認超時。");
            }
            catch (Exception ex)
            {
                // 處理其他讀取錯誤
                throw new Exception($"接收伺服器確認訊號錯誤: {ex.Message}", ex);
            }

        }

        // 用於處理連接失敗時的錯誤。停止計時器，啟用連接按鈕，關閉進度視窗，顯示錯誤訊息。
        private void HandleConnectionError(Exception ex)
        {
            connectionTimer.Stop();
            btnEnableTCP.Enabled = true;
            progressForm?.Close();
            MessageBox.Show($"連接失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // 計時器每秒觸發的事件處理器。更新進度視窗，並在計時結束時處理超時情況。
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
        private void btnDisableTCP_Click(object sender, EventArgs e) => Close();
    }
}