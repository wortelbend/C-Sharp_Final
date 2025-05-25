using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using P2PServer;
using System.Text;

namespace P2PChat
{
    // 聊天視窗主表單，處理網路連接和訊息傳輸
    public partial class ChatForm : Form
    {
        // 網路連接相關變數
        private string strHostname;
        private string strLocalIP;
        private IPAddress ipaLocal;
        private int localPort = 2000;
        private int remotePort = 2000;

        // 伺服器端連接變數
        private Socket sktListener;
        private Socket sktClient;
        private Thread thrListener;
        private Thread thrReceiver;
        private bool isListening = false;

        // 客戶端連接變數
        private Socket sktConnect;
        private bool isConnected = false;

        private TcpClient _connectedClient;
        private Form _parentForm;

        public ChatForm()
        {
            InitializeComponent();
            InitializeNetwork();
            SetupEventHandlers();
            this.FormClosing += ChatForm_FormClosing;
        }

        public ChatForm(TcpClient connectedClient)
        {
            InitializeComponent();
            _connectedClient = connectedClient;
            this.FormClosing += ChatForm_FormClosing;

            thrReceiver = new Thread(new ThreadStart(ReceiverThread));
            thrReceiver.IsBackground = true;
            thrReceiver.Start();

            AppendMessage("連線成功！");
            SetupEventHandlers();
        }

        public ChatForm(TcpClient connectedClient, Form parentForm)
        {
            InitializeComponent();
            _connectedClient = connectedClient;
            _parentForm = parentForm;
            this.FormClosing += ChatForm_FormClosing;

            thrReceiver = new Thread(new ThreadStart(ReceiverThread));
            thrReceiver.IsBackground = true;
            thrReceiver.Start();

            AppendMessage("連線成功！");
            SetupEventHandlers();
        }

        // 初始化網路設定
        private void InitializeNetwork()
        {
            strHostname = Dns.GetHostName();
            ipaLocal = Dns.Resolve(strHostname).AddressList[0];
            strLocalIP = ipaLocal.ToString();
        }

        // 設定所有按鈕和輸入框的事件處理器
        private void SetupEventHandlers()
        {
            btnSend.Click += btnSend_Click;
            txtMessage.KeyDown += txtMessage_KeyDown;
            txtMessage.TextChanged += txtMessage_TextChanged;
            btnclear.Click += btnclear_Click;
            btndisconnect.Click += btndisconnect_Click;
            btnpicture.Click += btnpicture_Click;
            btnreadpic.Click += btnreadpic_Click;

            if (btnchat1 != null) btnchat1.Click += btnchat_Click;
            if (btnchat2 != null) btnchat2.Click += btnchat_Click;
            if (btnchat3 != null) btnchat3.Click += btnchat_Click;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            if (lblword != null && !lblword.IsDisposed)
            {
                lblword.Text = $"可傳送字數：{txtMessage.Text.Length}/500";
            }
        }

        private void btnchat_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            string predefinedMessage = "";
            switch (clickedButton.Name)
            {
                case "btnchat1":
                    if (txtchat1 != null && !txtchat1.IsDisposed)
                        predefinedMessage = txtchat1.Text;
                    break;
                case "btnchat2":
                    if (txtchat2 != null && !txtchat2.IsDisposed)
                        predefinedMessage = txtchat2.Text;
                    break;
                case "btnchat3":
                    if (txtchat3 != null && !txtchat3.IsDisposed)
                        predefinedMessage = txtchat3.Text;
                    break;
            }

            if (string.IsNullOrEmpty(predefinedMessage)) return;

            if (_connectedClient != null && _connectedClient.Connected)
            {
                try
                {
                    byte[] data = Encoding.Unicode.GetBytes(predefinedMessage);
                    _connectedClient.GetStream().Write(data, 0, data.Length);
                    AppendMessage("我: " + predefinedMessage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("發送失敗: " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("未連接到對方，無法發送訊息", "發送失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SendMessage(string messageToSend = null)
        {
            string message = messageToSend ?? txtMessage.Text;
            if (string.IsNullOrEmpty(message)) return;

            if (message.Length > 500)
            {
                AppendMessage("系統提示：訊息長度超過上限（500字），無法傳送");
                return;
            }

            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message);

                if (_connectedClient != null && _connectedClient.Connected)
                {
                    _connectedClient.GetStream().Write(data, 0, data.Length);

                    if (!message.StartsWith("<IMAGE>"))
                    {
                        AppendMessage("我: " + message);
                    }

                    if (!message.StartsWith("<IMAGE>"))
                    {
                        txtMessage.Clear();
                    }
                }
                else if (isConnected)
                {
                    sktConnect.Send(data);
                    if (!message.StartsWith("<IMAGE>"))
                    {
                        AppendMessage("我: " + message);
                        txtMessage.Clear();
                    }
                }
                else if (sktClient != null && sktClient.Connected)
                {
                    sktClient.Send(data);
                    if (!message.StartsWith("<IMAGE>"))
                    {
                        AppendMessage("我: " + message);
                        txtMessage.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("未連接到對方，無法發送訊息", "發送失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("發送失敗: " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListenerThread()
        {
            try
            {
                while (isListening)
                {
                    sktClient = sktListener.Accept();
                    thrReceiver = new Thread(new ThreadStart(ReceiverThread));
                    thrReceiver.IsBackground = true;
                    thrReceiver.Start();
                    AppendMessage("接受了一個新的連接");
                }
            }
            catch
            {
                if (isListening)
                    AppendMessage("監聽器發生錯誤");
            }
        }

        private StringBuilder receiveBuffer = new StringBuilder();

        private void ReceiverThread()
        {
            if (_connectedClient == null || !_connectedClient.Connected)
            {
                AppendMessage("接收執行緒啟動失敗：未建立連接");
                return;
            }

            NetworkStream stream = _connectedClient.GetStream();
            byte[] buffer = new byte[4096];

            try
            {
                while (_connectedClient.Connected && !this.IsDisposed)
                {
                    int received = stream.Read(buffer, 0, buffer.Length);
                    if (received > 0)
                    {
                        string part = Encoding.Unicode.GetString(buffer, 0, received);
                        receiveBuffer.Append(part);

                        string bufferStr = receiveBuffer.ToString();

                        while (bufferStr.Contains("<IMAGE>") && bufferStr.Contains("</IMAGE>"))
                        {
                            int imgStart = bufferStr.IndexOf("<IMAGE>");
                            int imgEnd = bufferStr.IndexOf("</IMAGE>");
                            if (imgStart != -1 && imgEnd != -1 && imgEnd > imgStart)
                            {
                                string beforeImg = bufferStr.Substring(0, imgStart);
                                if (!string.IsNullOrWhiteSpace(beforeImg))
                                {
                                    AppendMessage("對方: " + beforeImg);
                                }

                                string imageMessage = bufferStr.Substring(imgStart, imgEnd + 8 - imgStart);
                                string base64Image = imageMessage.Substring(7, imageMessage.Length - 15);
                                byte[] imageBytes = Convert.FromBase64String(base64Image);

                                this.Invoke((Action)(() =>
                                {
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        if (pictureBox1.Image != null)
                                            pictureBox1.Image.Dispose();
                                        pictureBox1.Image = Image.FromStream(ms);
                                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                    }
                                }));

                                AppendMessage("對方傳送了圖片");

                                bufferStr = bufferStr.Substring(imgEnd + 8);
                                receiveBuffer.Clear();
                                receiveBuffer.Append(bufferStr);
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (bufferStr.Contains("<IMAGE_URL>") && bufferStr.Contains("</IMAGE_URL>"))
                        {
                            int urlStart = bufferStr.IndexOf("<IMAGE_URL>");
                            int urlEnd = bufferStr.IndexOf("</IMAGE_URL>");
                            if (urlStart != -1 && urlEnd != -1 && urlEnd > urlStart)
                            {
                                string imageUrl = bufferStr.Substring(urlStart + 11, urlEnd - urlStart - 11);

                                try
                                {
                                    using (WebClient client = new WebClient())
                                    {
                                        byte[] imageBytes = client.DownloadData(imageUrl);
                                        this.Invoke((Action)(() =>
                                        {
                                            using (MemoryStream ms = new MemoryStream(imageBytes))
                                            {
                                                if (pictureBox1.Image != null)
                                                    pictureBox1.Image.Dispose();
                                                pictureBox1.Image = Image.FromStream(ms);
                                                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                            }
                                        }));
                                    }
                                    AppendMessage("對方傳送了圖片連結");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("圖片連結處理失敗: " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                bufferStr = bufferStr.Substring(urlEnd + 12);
                                receiveBuffer.Clear();
                                receiveBuffer.Append(bufferStr);
                            }
                        }

                        if (!bufferStr.Contains("<IMAGE>") && !string.IsNullOrWhiteSpace(bufferStr))
                        {
                            if (bufferStr.Trim() == "<DISCONNECT>")
                            {
                                AppendMessage("對方已經斷開連接");
                                if (_connectedClient != null)
                                {
                                    try { _connectedClient.Close(); } catch { }
                                    _connectedClient = null;
                                }
                                DisableControls();
                                receiveBuffer.Clear();
                                break;
                            }
                            else
                            {
                                AppendMessage("對方: " + bufferStr);
                                receiveBuffer.Clear();
                            }
                        }
                    }
                    else if (received == 0)
                    {
                        AppendMessage("連接已斷開 (對方關閉)");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (_connectedClient?.Connected == true)
                {
                    AppendMessage("接收錯誤: " + ex.Message);
                }
                else
                {
                    AppendMessage("連接已斷開");
                }
            }
            finally
            {
                if (_connectedClient?.Connected == true)
                {
                    try { _connectedClient.Close(); } catch { }
                }
                _connectedClient = null;
            }
        }

        private void StopListening()
        {
            isListening = false;
            if (sktListener != null)
            {
                sktListener.Close();
                sktListener = null;
            }
            if (thrListener != null)
            {
                thrListener.Abort();
                thrListener = null;
            }
            AppendMessage("停止監聽");
        }

        private void DisconnectClient()
        {
            isConnected = false;
            if (sktConnect != null)
            {
                sktConnect.Close();
                sktConnect = null;
            }
            if (_connectedClient == null)
            {
                AppendMessage("已斷開連接");
            }
        }

        private void AppendMessage(string message, bool clear = false)
        {
            if (rtbMessages == null || rtbMessages.IsDisposed || this.IsDisposed || this.Disposing || !rtbMessages.IsHandleCreated) return;

            if (rtbMessages.InvokeRequired)
            {
                try
                {
                    rtbMessages.Invoke((MethodInvoker)delegate
                    {
                        if (rtbMessages != null && !rtbMessages.IsDisposed && !this.IsDisposed && !this.Disposing && rtbMessages.IsHandleCreated)
                        {
                            if (clear)
                                rtbMessages.Clear();
                            else
                                rtbMessages.AppendText(message + Environment.NewLine);
                        }
                    });
                }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
            }
            else
            {
                if (rtbMessages != null && !rtbMessages.IsDisposed && !this.IsDisposed && !this.Disposing && rtbMessages.IsHandleCreated)
                {
                    if (clear)
                        rtbMessages.Clear();
                    else
                        rtbMessages.AppendText(message + Environment.NewLine);
                }
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            AppendMessage(null, true);
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            DialogResult result = MessageBox.Show("確定要斷開連接嗎？", "確認斷開", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (_connectedClient != null && _connectedClient.Connected)
                {
                    try
                    {
                        SendMessage("<DISCONNECT>");
                        Thread.Sleep(100);
                    }
                    catch { }
                }

                if (_connectedClient != null)
                {
                    try { _connectedClient.Close(); } catch { }
                    _connectedClient = null;
                }

                if (thrReceiver != null && thrReceiver.IsAlive)
                {
                    try { thrReceiver.Join(100); } catch { }
                }

                if (_parentForm != null && !_parentForm.IsDisposed)
                {
                    if (_parentForm.InvokeRequired)
                    {
                        _parentForm.Invoke((MethodInvoker)delegate
                        {
                            if (!_parentForm.IsDisposed && !_parentForm.Disposing) _parentForm.Show();
                        });
                    }
                    else
                    {
                        if (!_parentForm.IsDisposed && !_parentForm.Disposing) _parentForm.Show();
                    }
                }

                this.FormClosing -= ChatForm_FormClosing;
                this.Close();
            }
        }

        private void btndisconnect_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 開啟圖片傳送視窗
        private void btnpicture_Click(object sender, EventArgs e)
        {
            Pictureform pictureForm = new Pictureform(_connectedClient);
            pictureForm.Show();
        }

        // 停用所有控制項，用於斷線時
        private void DisableControls()
        {
            if (txtMessage != null && btnSend != null)
            {
                txtMessage.Enabled = false;
                btnSend.Enabled = false;
            }
            if (btnchat1 != null && !btnchat1.IsDisposed) btnchat1.Enabled = false;
            if (btnchat2 != null && !btnchat2.IsDisposed) btnchat2.Enabled = false;
            if (btnchat3 != null && !btnchat3.IsDisposed) btnchat3.Enabled = false;
            if (btnpicture != null && !btnpicture.IsDisposed) btnpicture.Enabled = false;
            if (btnreadpic != null && !btnreadpic.IsDisposed) btnreadpic.Enabled = false;
        }

        // 開啟圖片預覽視窗
        private void btnreadpic_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                P2PServer.ViewPictureForm viewForm = new P2PServer.ViewPictureForm();
                viewForm.SetImage(pictureBox1.Image);
                viewForm.Show();
            }
            else
            {
                MessageBox.Show("目前沒有圖片可以查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}