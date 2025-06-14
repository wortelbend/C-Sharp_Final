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

        // vvv--- 新增變數 ---vvv
        private Image myAvatar;
        private Image remoteAvatar;
        // ^^^--- 新增變數 ---^^^

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

            AppendMessage("連線成功！", false);

            SetupEventHandlers();
        }

        // 接收頭像
        public ClientChatForm(TcpClient connectedClient, Form parentForm, Image avatar)
        {
            InitializeComponent();
            _connectedClient = connectedClient;
            _parentForm = parentForm;
            this.myAvatar = avatar;
            this.FormClosing += ChatForm_FormClosing;

            thrReceiver = new Thread(new ThreadStart(ReceiverThread));
            thrReceiver.IsBackground = true;
            thrReceiver.Start();

            AppendMessage("連線成功！", false);
            SendAvatar();

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

            AppendMessage("連線成功！", false);

            SetupEventHandlers();
        }

        //傳送頭像
        private void SendAvatar()
        {
            if (myAvatar == null || _connectedClient == null || !_connectedClient.Connected) return;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    myAvatar.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    string base64Image = Convert.ToBase64String(imageBytes);
                    string message = $"<AVATAR>{base64Image}</AVATAR>";
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    _connectedClient.GetStream().Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("傳送頭像失敗: " + ex.Message);
            }
        }

        // 初始化網路設定，獲取本機主機名稱和 IP 位址
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

        // 這個方法處理訊息輸入框的按鍵按下事件。若使用者按下 Enter 鍵，會觸發 SendMessage 方法來發送訊息，並阻止 Enter 鍵產生換行符
        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // 訊息輸入框文字改變。即時更新顯示可傳送字數的標籤
        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            if (lblword != null && !lblword.IsDisposed)
            {
                lblword.Text = $"已輸入字數：{txtMessage.Text.Length}";
            }
        }

        // 預設訊息按鈕的點擊。將預設訊息使用Tcp傳送給對方
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
                    AppendMessageWithAvatar(myAvatar, predefinedMessage);
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

        // 將文字訊息傳送給對方。會檢查訊息長度，透過網路連接傳送
        // 支援透過 TcpClient 或 Socket 傳送，並在訊息發送後更新聊天記錄
        private void SendMessage(string messageToSend = null)
        {
            string message = messageToSend ?? txtMessage.Text;
            if (string.IsNullOrEmpty(message)) return;

            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message);

                if (_connectedClient != null && _connectedClient.Connected)
                {
                    _connectedClient.GetStream().Write(data, 0, data.Length);

                    if (!message.StartsWith("<IMAGE>"))
                    {
                        AppendMessageWithAvatar(myAvatar, message);
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
                        AppendMessageWithAvatar(myAvatar, message);
                        txtMessage.Clear();
                    }
                }
                else if (sktClient != null && sktClient.Connected)
                {
                    sktClient.Send(data);
                    if (!message.StartsWith("<IMAGE>"))
                    {
                        AppendMessageWithAvatar(myAvatar, message);
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

        // 監聽傳入的連接請求。一旦有新的連接，會接受連接並啟動一個新的接收執行緒來處理該客戶端的訊息
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
                    AppendMessage("接受了一個新的連接", false);
                }
            }
            catch
            {
                if (isListening)
                    AppendMessage("監聽器發生錯誤", false);
            }
        }

        private StringBuilder receiveBuffer = new StringBuilder();

        // 用於從已連接的客戶端接收訊息
        // 會持續讀取網路資料流中的資料，並解析接收到的訊息，包括文字訊息和圖片（Base64 編碼或圖片連結）。接收到的訊息會顯示在聊天視窗中，並處理斷開連接的特殊指令
        private void ReceiverThread()
        {
            if (_connectedClient == null || !_connectedClient.Connected)
            {
                AppendMessage("接收執行緒啟動失敗：未建立連接", false);
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

                        // --- 新增：處理頭像的邏輯 ---
                        while (bufferStr.Contains("<AVATAR>") && bufferStr.Contains("</AVATAR>"))
                        {
                            int avatarStart = bufferStr.IndexOf("<AVATAR>");
                            int avatarEnd = bufferStr.IndexOf("</AVATAR>");
                            if (avatarStart != -1 && avatarEnd > avatarStart)
                            {
                                string base64Avatar = bufferStr.Substring(avatarStart + 8, avatarEnd - (avatarStart + 8));
                                byte[] avatarBytes = Convert.FromBase64String(base64Avatar);
                                using (MemoryStream ms = new MemoryStream(avatarBytes))
                                {
                                    // 複製一份，避免GDI+泛型錯誤
                                    this.remoteAvatar = new Bitmap(Image.FromStream(ms));
                                }

                                // 從緩衝區移除頭像資料
                                bufferStr = bufferStr.Substring(avatarEnd + 9);
                                receiveBuffer.Clear();
                                receiveBuffer.Append(bufferStr);
                            }
                            else break;
                        }

                        while (bufferStr.Contains("<IMAGE>") && bufferStr.Contains("</IMAGE>"))
                        {
                            int imgStart = bufferStr.IndexOf("<IMAGE>");
                            int imgEnd = bufferStr.IndexOf("</IMAGE>");
                            if (imgStart != -1 && imgEnd != -1 && imgEnd > imgStart)
                            {
                                string beforeImg = bufferStr.Substring(0, imgStart);
                                if (!string.IsNullOrWhiteSpace(beforeImg))
                                {
                                    AppendMessageWithAvatar(remoteAvatar, beforeImg);
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

                                AppendMessage("對方傳送了圖片", false);

                                bufferStr = bufferStr.Substring(imgEnd + 8);
                                receiveBuffer.Clear();
                                receiveBuffer.Append(bufferStr);
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (!bufferStr.Contains("<IMAGE>") && !bufferStr.Contains("<AVATAR>") && !string.IsNullOrWhiteSpace(bufferStr))
                        {
                            if (bufferStr.Trim() == "<DISCONNECT>")
                            {
                                AppendMessage("對方已經斷開連接", false);
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
                                AppendMessageWithAvatar(remoteAvatar, bufferStr);
                                receiveBuffer.Clear();
                            }
                        }
                    }
                    else if (received == 0)
                    {
                        AppendMessage("連接已斷開 (對方關閉)", false);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (_connectedClient?.Connected == true)
                {
                    AppendMessage("接收錯誤: " + ex.Message, false);
                }
                else
                {
                    AppendMessage("連接已斷開", false);
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

        // 停止監聽傳入的連接請求，並清理相關的 Socket 和執行緒資源
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
            AppendMessage("停止監聽", false);
        }

        // 斷開與客戶端的連接，並清理相關的 Socket 資源
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
                AppendMessage("已斷開連接", false);
            }
        }

        //顯示頭像和訊息
        private void AppendMessageWithAvatar(Image avatar, string message)
        {
            if (flpMessages.InvokeRequired)
            {
                flpMessages.Invoke((MethodInvoker)delegate {
                    AppendMessageWithAvatar(avatar, message);
                });
                return;
            }
            Panel messagePanel = new Panel
            {
                Width = flpMessages.ClientSize.Width - 25,
                AutoSize = true,
                Margin = new Padding(5)
            };
            PictureBox picAvatar = new PictureBox
            {
                Size = new Size(50, 50),
                Location = new Point(3, 3),
                SizeMode = PictureBoxSizeMode.Zoom,
            };

            if (avatar != null)
            {
                picAvatar.Image = new Bitmap(avatar);
            }
            Label lblMessage = new Label
            {
                Text = message,
                Location = new Point(55, 5),
                AutoSize = true,
                MaximumSize = new Size(messagePanel.Width - 50, 0)
            };
            messagePanel.Controls.Add(picAvatar);
            messagePanel.Controls.Add(lblMessage);
            flpMessages.Controls.Add(messagePanel);
            flpMessages.ScrollControlIntoView(messagePanel);
        }
        private void AppendMessage(string message, bool clear = false)
        {
            if (flpMessages.InvokeRequired)
            {
                flpMessages.Invoke((MethodInvoker)delegate {
                    AppendMessage(message, clear);
                });
                return;
            }

            if (clear)
            {
                flpMessages.Controls.Clear();
                return;
            }
            Label lblSystemMessage = new Label
            {
                Text = message,
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font(this.Font, FontStyle.Italic),
                Margin = new Padding(10),
                Padding = new Padding(5)
            };

            flpMessages.Controls.Add(lblSystemMessage);
            flpMessages.ScrollControlIntoView(lblSystemMessage);
        }

        //清空聊天訊息框中的所有訊息
        private void btnclear_Click(object sender, EventArgs e)
        {
            if (flpMessages != null)
            {
                flpMessages.Controls.Clear();
            }
        }

        // 處理聊天視窗關閉的事件。彈出確認對話框，如果確認斷開連接，則會向對方發送斷開連接的訊息
        // 關閉連接，並將父視窗重新顯示
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

        // 斷開連接按鈕
        private void btndisconnect_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 傳送圖片按鈕的點擊事件
        // 開啟一個新的圖片傳送視窗 讓使用者選擇圖片並傳送
        // 新增 會等待圖片視窗關閉，並接收回傳的圖片以顯示在本地
        private void btnpicture_Click(object sender, EventArgs e)
        {
            using (formpicture pictureForm = new formpicture(_connectedClient))
            {
                if (pictureForm.ShowDialog(this) == DialogResult.OK)
                {
                    Image receivedImage = pictureForm.ClonedImage;

                    if (receivedImage != null)
                    {
                        if (pictureBox2.Image != null)
                        {
                            pictureBox2.Image.Dispose();
                        }
                        pictureBox2.Image = receivedImage;
                        pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

                        AppendMessage("您傳送了一張圖片。", false);
                    }
                }
            }
        }

        // 圖片按鈕
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

        // 表情符號按鈕
        private void btnemoji_Click(object sender, EventArgs e)
        {
            try
            {
                Form emojiForm = new Form
                {
                    FormBorderStyle = FormBorderStyle.None,
                    StartPosition = FormStartPosition.Manual,
                    ShowInTaskbar = false,
                    TopMost = true,
                    Padding = new Padding(3)
                };

                FlowLayoutPanel flowPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    FlowDirection = FlowDirection.LeftToRight,
                    WrapContents = true,
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
                        Font = new Font(this.Font.FontFamily, 16F, FontStyle.Regular),
                        Size = new Size(48, 48),
                        FlatStyle = FlatStyle.Flat,
                        Margin = new Padding(2)
                    };

                    emojiButton.FlatAppearance.BorderSize = 0;
                    emojiButton.Click += (s, args) =>
                    {
                        if (txtMessage != null && !txtMessage.IsDisposed)
                        {
                            var selectionIndex = txtMessage.SelectionStart;
                            txtMessage.Text = txtMessage.Text.Insert(selectionIndex, emoji);
                            txtMessage.SelectionStart = selectionIndex + emoji.Length;
                            txtMessage.Focus();
                        }
                        emojiForm.Close();
                    };

                    flowPanel.Controls.Add(emojiButton);
                }

                emojiForm.Size = new Size(300, 200);
                emojiForm.Controls.Add(flowPanel);
                emojiForm.Location = btnemoji.PointToScreen(new Point(0, btnemoji.Height));
                emojiForm.Deactivate += (s, args) => emojiForm.Close();
                emojiForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"開啟表情符號選單時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 禁用聊天視窗中的一些控制項，例如訊息輸入框和傳送按鈕
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