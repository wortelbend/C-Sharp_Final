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
    public partial class ClientConnectForm : Form
    {
        // TCP相關變數
        private System.Windows.Forms.Timer connectionTimer;
        private int remainingSeconds = 30;
        private TcpClient tcpClient;
        private Form progressForm;
        private Image myAvatar;

        public ClientConnectForm()
        {
            InitializeComponent();
            InitializeTimer();
            btnEnableTCP.Click += btnEnableTCP_Click;
            this.VisibleChanged += Form1_VisibleChanged;
            btnavater.Click += btnavatar_Click;
        }

        // 新增頭像 限制5MB  限制檔案類型.jpg .jpeg .png
        private void btnavatar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
                ofd.Title = "請選擇您的頭像";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    const long maxSizeInBytes = 5 * 1024 * 1024;
                    FileInfo fileInfo = new FileInfo(ofd.FileName);
                    if (fileInfo.Length > maxSizeInBytes)
                    {
                        MessageBox.Show(
                            $"選擇的頭像檔案大小為 {(fileInfo.Length / 1024.0 / 1024.0):F2} MB，\n已超過 5MB 的上限，請選擇較小的圖片。",
                            "檔案過大",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }
                    myAvatar = Image.FromFile(ofd.FileName);
                    if (this.Controls.ContainsKey("pictureBox2") && this.Controls["pictureBox2"] is PictureBox pbox)
                    {
                        if (pbox.Image != null) pbox.Image.Dispose();
                        pbox.Image = new Bitmap(myAvatar);
                        pbox.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
            }
        }

        // 初始化計時器，每秒觸發一次
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

        // 顯示連接進度視窗，含倒數計時器
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
            progressForm.FormClosed += ProgressForm_FormClosed;
            progressForm.Show();
        }

        // 當進度視窗關閉時，重新啟用連接按鈕並清除 progressForm 引用
        private void ProgressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (btnEnableTCP != null && !btnEnableTCP.IsDisposed)
            {
                btnEnableTCP.Enabled = true;
            }
            progressForm = null;
        }

        // 更新視窗中顯示的倒數計時
        private void UpdateProgressForm()
        {
            if (progressForm?.IsDisposed == false)
            {
                ((Label)progressForm.Controls[0]).Text = $"正在連接伺服器...\n剩餘時間：{remainingSeconds}秒";
            }
        }

        // 嘗試建立一個 TCP 連接，在連接過程中顯示進度視窗和倒數計時
        private async void btnEnableTCP_Click(object sender, EventArgs e)
        {
            if (myAvatar == null)
            {
                MessageBox.Show("您尚未選擇頭像，請點擊選擇頭像按鈕選擇圖片。", "未選擇頭像", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtClientIP.Text))
            {
                MessageBox.Show("請輸入IP地址", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtClientPORT.Text))
            {
                MessageBox.Show("請輸入Port", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                new ClientChatForm(tcpClient, this, myAvatar).Show();
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
                    MessageBox.Show("找無伺服器", "連線失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (ex is TimeoutException)
                {
                    connectionTimer.Stop();
                    btnEnableTCP.Enabled = true;
                    progressForm?.Close();
                    MessageBox.Show("連接超時，伺服器無回應", "連線失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    HandleConnectionError(ex);
                }
            }
        }

        // 等待伺服器的連線確認訊號。會從網路資料流中讀取資料，直到收到特定的確認訊號（例如 "<ACCEPT_CHAT>"）或超時
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
                            throw new SocketException((int)SocketError.ConnectionAborted);
                        }
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException("等待伺服器確認超時。");
            }
            catch (Exception ex)
            {
                throw new Exception($"接收伺服器確認訊號錯誤: {ex.Message}", ex);
            }
        }

        // 連接失敗錯誤。停止計時器，啟用連接按鈕，關閉進度視窗，顯示錯誤訊息。
        private void HandleConnectionError(Exception ex)
        {
            connectionTimer.Stop();
            btnEnableTCP.Enabled = true;
            progressForm?.Close();
            MessageBox.Show($"連接失敗：伺服器拒絕連線", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // 計時器每秒觸發。更新進度視窗，並在計時結束時處理超時情況
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