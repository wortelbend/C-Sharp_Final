using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.IO;
using P2PChat;
using System.Runtime.InteropServices;

namespace P2PChat
{
    public partial class ClientChatForm : Form
    {
        // Windows API 常數
        private const int KEYEVENTF_KEYUP = 0x0002;
        private const int VK_LWIN = 0x5B;
        private const int VK_OEM_PERIOD = 0xBE;

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        // 基本網路變數
        private string strHostname;
        private string strLocalIP;
        private IPAddress ipaLocal;
        private int localPort = 2000;
        private int remotePort = 2000;

        // 伺服器端變數
        private Socket sktListener;
        private Socket sktClient;
        private Thread thrListener;
        private Thread thrReceiver;
        private bool isListening = false;

        // 客戶端變數
        private Socket sktConnect;
        private bool isConnected = false;

        private TcpClient _connectedClient;
        private Form _parentForm;

        public ClientChatForm()
        {
            InitializeComponent();
            InitializeNetwork();
            SetupEventHandlers();
            this.FormClosing += ChatForm_FormClosing;
        }

        public ClientChatForm(TcpClient connectedClient)
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

        public ClientChatForm(TcpClient connectedClient, Form parentForm)
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

        // 初始化網路設定，獲取本機主機名稱和 IP 位址。
        private void InitializeNetwork()
        {
            strHostname = Dns.GetHostName();
            ipaLocal = Dns.Resolve(strHostname).AddressList[0];
            strLocalIP = ipaLocal.ToString();
        }

        // 聊天視窗中所有按鈕和文字輸入框的事件處理器
        private void SetupEventHandlers()
        {
            btnSend.Click += btnSend_Click;
            txtMessage.KeyDown += txtMessage_KeyDown;
            txtMessage.TextChanged += txtMessage_TextChanged;
            btnclear.Click += btnclear_Click;
            btndisconnect.Click += btndisconnect_Click;
            btnpicture.Click += btnpicture_Click;
            btnreadpic.Click += btnreadpic_Click;
            btnemoji.Click += btnemoji_Click;

            if (btnchat1 != null) btnchat1.Click += btnchat_Click;
            if (btnchat2 != null) btnchat2.Click += btnchat_Click;
            if (btnchat3 != null) btnchat3.Click += btnchat_Click;
        }

        // 發送訊息
        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        // 這個方法處理訊息輸入框的按鍵按下事件。若使用者按下 Enter 鍵，會觸發 SendMessage 方法來發送訊息，並阻止 Enter 鍵產生換行符。
        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // 訊息輸入框文字改變。即時更新顯示可傳送字數的標籤。
        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            if (lblword != null && !lblword.IsDisposed)
            {
                lblword.Text = $"可傳送字數限制：{txtMessage.Text.Length}/500";
            }
        }

        // 預設訊息按鈕的點擊。將預設訊息使用Tcp傳送給對方。
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

        // 將文字訊息傳送給對方。會檢查訊息長度，透過網路連接傳送。
        // 支援透過 TcpClient 或 Socket 傳送，並在訊息發送後更新聊天記錄。
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

        // 監聽傳入的連接請求。一旦有新的連接，會接受連接並啟動一個新的接收執行緒來處理該客戶端的訊息。
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

        // 用於從已連接的客戶端接收訊息。
        // 會持續讀取網路資料流中的資料，並解析接收到的訊息，包括文字訊息和圖片（Base64 編碼或圖片連結）。接收到的訊息會顯示在聊天視窗中，並處理斷開連接的特殊指令。
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

        // 停止監聽傳入的連接請求，並清理相關的 Socket 和執行緒資源。
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

        // 斷開與客戶端的連接，並清理相關的 Socket 資源。
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

        // 在聊天訊息框中添加新的訊息，並可選擇清空現有訊息。
        private void AppendMessage(string message, bool clear = false)
        {
            if (rtbMessages == null || rtbMessages.IsDisposed || this.IsDisposed || this.Disposing || !rtbMessages.IsHandleCreated) return;

            // 將 isAtBottom 的判斷移到 Invoke 內部
            // 這樣可以確保所有對 rtbMessages 的存取都在 UI 執行緒上進行
            if (rtbMessages.InvokeRequired)
            {
                try
                {
                    rtbMessages.Invoke((MethodInvoker)delegate
                    {
                        if (rtbMessages != null && !rtbMessages.IsDisposed && !this.IsDisposed && !this.Disposing && rtbMessages.IsHandleCreated)
                        {
                            // 在 UI 執行緒中判斷是否在底部
                            bool isAtBottom = rtbMessages.TextLength == rtbMessages.SelectionStart;

                            if (clear)
                                rtbMessages.Clear();
                            else
                                rtbMessages.AppendText(message + Environment.NewLine);

                            // 只有當原本就在底部，或是清空後添加新訊息時，才自動滾動
                            if (isAtBottom || clear)
                            {
                                rtbMessages.SelectionStart = rtbMessages.Text.Length;
                                rtbMessages.ScrollToCaret();
                            }
                        }
                    });
                }
                catch (ObjectDisposedException) { /* 忽略已處置物件的例外 */ }
                catch (InvalidOperationException) { /* 忽略無效操作的例外 */ }
            }
            else
            {
                // 在 UI 執行緒中判斷是否在底部
                bool isAtBottom = rtbMessages.TextLength == rtbMessages.SelectionStart;

                if (rtbMessages != null && !rtbMessages.IsDisposed && !this.IsDisposed && !this.Disposing && rtbMessages.IsHandleCreated)
                {
                    if (clear)
                        rtbMessages.Clear();
                    else
                        rtbMessages.AppendText(message + Environment.NewLine);

                    // 只有當原本就在底部，或是清空後添加新訊息時，才自動滾動
                    if (isAtBottom || clear)
                    {
                        rtbMessages.SelectionStart = rtbMessages.Text.Length;
                        rtbMessages.ScrollToCaret();
                    }
                }
            }
        }

        // 會清空聊天訊息框中的所有訊息。
        private void btnclear_Click(object sender, EventArgs e)
        {
            AppendMessage(null, true);
        }

        // 處理聊天視窗關閉的事件。彈出確認對話框，如果確認斷開連接，則會向對方發送斷開連接的訊息，
        // 關閉連接，並將父視窗重新顯示。
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

        // 這個方法處理斷開連接按鈕的點擊事件，它會觸發視窗關閉事件，
        private void btndisconnect_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 這個方法處理傳送圖片按鈕的點擊事件。
        private void btnpicture_Click(object sender, EventArgs e)
        {
            formpicture pictureForm = new formpicture(_connectedClient);
            pictureForm.Show();
        }

        // 查看圖片按鈕的點擊事件。
        private void btnreadpic_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                P2PChat.ClientViewPictureForm viewForm = new P2PChat.ClientViewPictureForm();
                viewForm.SetImage(pictureBox1.Image);
                viewForm.Show();
            }
            else
            {
                MessageBox.Show("目前沒有圖片可以查看", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // 表情符號按鈕的點擊事件。
        private void btnemoji_Click(object sender, EventArgs e)
        {
            try
            {
                Form emojiForm = new Form
                {
                    FormBorderStyle = FormBorderStyle.None,
                    StartPosition = FormStartPosition.Manual,
                    ShowInTaskbar = false,
                    TopMost = true
                };

                FlowLayoutPanel flowPanel = new FlowLayoutPanel
                {
                    AutoScroll = true,
                    FlowDirection = FlowDirection.LeftToRight,
                    WrapContents = true,
                    Size = new Size(300, 200),
                    Padding = new Padding(5),
                    BackColor = Color.White
                };

                string[] emojis = new string[]
                {
                    "😊", "😂", "❤️", "👍", "🎉","😍", "😭", "🙏", "😎", "🤔","😡", "😴", "🤗", "😱", "😇","😘", "🥰",
                };

                foreach (string emoji in emojis)
                {
                    Button emojiButton = new Button
                    {
                        Text = emoji,
                        Font = new Font("Segoe UI Emoji", 16F),
                        Size = new Size(50, 50),
                        FlatStyle = FlatStyle.Flat,
                        Margin = new Padding(2)
                    };

                    emojiButton.FlatAppearance.BorderSize = 0;
                    emojiButton.Click += (s, e) =>
                    {
                        if (txtMessage != null && !txtMessage.IsDisposed)
                        {
                            txtMessage.SelectedText = emoji;
                            txtMessage.Focus();
                        }
                        emojiForm.Close();
                    };

                    flowPanel.Controls.Add(emojiButton);
                }

                emojiForm.Controls.Add(flowPanel);
                emojiForm.Size = flowPanel.Size;
                emojiForm.Location = btnemoji.PointToScreen(new Point(0, btnemoji.Height));
                emojiForm.Deactivate += (s, e) => emojiForm.Close();
                emojiForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"開啟表情符號選單時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 禁用聊天視窗中的一些控制項，例如訊息輸入框和傳送按鈕。
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
        }
    }
}