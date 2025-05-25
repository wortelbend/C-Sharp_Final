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
using P2PServer;

namespace P2PChat
{
    public partial class ChatForm : Form
    {
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

        // 新增一個欄位來儲存傳入的 TcpClient
        private TcpClient _connectedClient;
        private Form _parentForm; // 新增欄位來儲存父視窗參考

        public ChatForm()
        {
            InitializeComponent();
            InitializeNetwork();
            SetupEventHandlers();
            this.FormClosing += ChatForm_FormClosing;
        }

        // 新增一個建構子，接收一個已連接的 TcpClient 物件
        public ChatForm(TcpClient connectedClient)
        {
            InitializeComponent();
            _connectedClient = connectedClient;
            this.FormClosing += ChatForm_FormClosing;

            // 啟動接收訊息的執行緒
            thrReceiver = new Thread(new ThreadStart(ReceiverThread));
            thrReceiver.IsBackground = true; // 設定為背景執行緒
            thrReceiver.Start();

            AppendMessage("連線成功！");

            SetupEventHandlers();
        }

        // 新增一個建構子，接收一個已連接的 TcpClient 物件和父視窗參考
        public ChatForm(TcpClient connectedClient, Form parentForm)
        {
            InitializeComponent();
            _connectedClient = connectedClient;
            _parentForm = parentForm; // 儲存父視窗參考
            this.FormClosing += ChatForm_FormClosing;

            // 啟動接收訊息的執行緒
            thrReceiver = new Thread(new ThreadStart(ReceiverThread));
            thrReceiver.IsBackground = true; // 設定為背景執行緒
            thrReceiver.Start();

            AppendMessage("連線成功！");

            SetupEventHandlers();
        }

        private void InitializeNetwork()
        {
            // 獲取本機網路資訊
            strHostname = Dns.GetHostName();
            ipaLocal = Dns.Resolve(strHostname).AddressList[0];
            strLocalIP = ipaLocal.ToString();
        }

        private void SetupEventHandlers()
        {
            btnSend.Click += btnSend_Click;
            txtMessage.KeyDown += txtMessage_KeyDown;
            txtMessage.TextChanged += txtMessage_TextChanged;  // 添加 TextChanged 事件
            btnclear.Click += btnclear_Click;
            btndisconnect.Click += btndisconnect_Click;
            btnpicture.Click += btnpicture_Click;  // 添加圖片按鈕事件
            btnreadpic.Click += btnreadpic_Click;  // 添加查看圖片按鈕事件

            if (btnchat1 != null) btnchat1.Click += btnchat_Click;
            if (btnchat2 != null) btnchat2.Click += btnchat_Click;
            if (btnchat3 != null) btnchat3.Click += btnchat_Click;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage(); // 調用新的發送訊息方法
        }

        // 處理 txtMessage 中的 Enter 鍵按下事件
        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage(); // 調用新的發送訊息方法
                e.Handled = true; // 阻止發出 Enter 鍵的預設音效
                e.SuppressKeyPress = true; // 阻止將 Enter 鍵傳遞給基礎控制項
            }
        }

        // 添加 TextChanged 事件處理方法
        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            if (lblword != null && !lblword.IsDisposed)
            {
                lblword.Text = $"可傳送字數：{txtMessage.Text.Length}/500";
            }
        }

        // 創建一個通用的按鈕點擊事件處理器
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

        // 修改 SendMessage 方法，使其可以選擇性地接受一個字串參數
        private void SendMessage(string messageToSend = null)
        {
            string message = messageToSend ?? txtMessage.Text; // 如果傳入了 messageToSend，則使用它，否則使用 txtMessage.Text
            if (string.IsNullOrEmpty(message)) return;

            // 檢查訊息長度是否超過 500 字
            if (message.Length > 500)
            {
                AppendMessage("系統提示：訊息長度超過上限（500字），無法傳送");
                return;
            }

            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message);

                // 檢查是否使用傳入的 TcpClient
                if (_connectedClient != null && _connectedClient.Connected)
                {
                    _connectedClient.GetStream().Write(data, 0, data.Length); // 使用 GetStream() 獲取 NetworkStream

                    // 如果是圖片訊息，不顯示在聊天記錄中
                    if (!message.StartsWith("<IMAGE>"))
                    {
                        AppendMessage("我: " + message); // 發送方自己顯示訊息
                    }

                    if (!message.StartsWith("<IMAGE>"))
                    {
                        txtMessage.Clear();
                    }
                }
                // 保留原有的 Socket 邏輯作為後備 (如果沒有使用帶 TcpClient 的建構子)
                else if (isConnected) // 客戶端模式 (雖然在 serverchat 中，但如果作為客戶端連線時使用)
                {
                    sktConnect.Send(data);
                    if (!message.StartsWith("<IMAGE>"))
                    {
                        AppendMessage("我: " + message); // 發送方自己顯示訊息
                        txtMessage.Clear();
                    }
                }
                else if (sktClient != null && sktClient.Connected) // 伺服器模式（接受單個客戶端）
                {
                    sktClient.Send(data);
                    if (!message.StartsWith("<IMAGE>"))
                    {
                        AppendMessage("我: " + message); // 發送方自己顯示訊息
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
                    thrReceiver.IsBackground = true; // 設定為背景執行緒
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
            byte[] buffer = new byte[4096]; // 緩衝區

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

                        // 處理所有 <IMAGE>...</IMAGE> 圖片訊息
                        while (bufferStr.Contains("<IMAGE>") && bufferStr.Contains("</IMAGE>"))
                        {
                            int imgStart = bufferStr.IndexOf("<IMAGE>");
                            int imgEnd = bufferStr.IndexOf("</IMAGE>");
                            if (imgStart != -1 && imgEnd != -1 && imgEnd > imgStart)
                            {
                                // <IMAGE>前面如果有純文字，先顯示出來
                                string beforeImg = bufferStr.Substring(0, imgStart);
                                if (!string.IsNullOrWhiteSpace(beforeImg))
                                {
                                    AppendMessage("對方: " + beforeImg);
                                }

                                // 抓出這段圖片訊息
                                string imageMessage = bufferStr.Substring(imgStart, imgEnd + 8 - imgStart);
                                string base64Image = imageMessage.Substring(7, imageMessage.Length - 15);
                                byte[] imageBytes = Convert.FromBase64String(base64Image);

                                // 1. 顯示圖片
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

                                // 2. 在 rtbMessages 顯示提示
                                AppendMessage("對方傳送了圖片");

                                // 處理剩下未處理的訊息
                                bufferStr = bufferStr.Substring(imgEnd + 8);
                                receiveBuffer.Clear();
                                receiveBuffer.Append(bufferStr);
                            }
                            else
                            {
                                // 目前buffer不足一個完整圖片訊息
                                break;
                            }
                        }

                        // 處理圖片連結
                        if (bufferStr.Contains("<IMAGE_URL>") && bufferStr.Contains("</IMAGE_URL>"))
                        {
                            int urlStart = bufferStr.IndexOf("<IMAGE_URL>");
                            int urlEnd = bufferStr.IndexOf("</IMAGE_URL>");
                            if (urlStart != -1 && urlEnd != -1 && urlEnd > urlStart)
                            {
                                // 提取圖片連結
                                string imageUrl = bufferStr.Substring(urlStart + 11, urlEnd - urlStart - 11);

                                try
                                {
                                    // 下載並顯示圖片
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

                                // 處理剩下未處理的訊息
                                bufferStr = bufferStr.Substring(urlEnd + 12);
                                receiveBuffer.Clear();
                                receiveBuffer.Append(bufferStr);
                            }
                        }

                        // 剩下不是圖片的都視為純文字，顯示在聊天視窗
                        if (!bufferStr.Contains("<IMAGE>") && !string.IsNullOrWhiteSpace(bufferStr))
                        {
                            // 避免 <DISCONNECT> 等特殊指令
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
            // 這個方法主要用於清理內部的 Socket 連接，如果使用傳入的 TcpClient 則不需要在這裡關閉
            isConnected = false;
            if (sktConnect != null)
            {
                sktConnect.Close();
                sktConnect = null;
            }
            if (thrReceiver != null) // 如果 ReceiverThread 是由內部連接觸發的
            {
                // thrReceiver.Abort(); // 不再使用 Abort，等待執行緒自然結束
                // thrReceiver = null;
            }
            if (_connectedClient == null) // 如果沒有傳入的 TcpClient，才顯示斷開訊息
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
            // 取消預設的關閉行為
            e.Cancel = true;

            // 顯示確認對話框
            DialogResult result = MessageBox.Show("確定要斷開連接嗎？", "確認斷開", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 只有在連接仍然有效時才嘗試發送斷開訊息
                if (_connectedClient != null && _connectedClient.Connected)
                {
                    try
                    {
                        SendMessage("<DISCONNECT>"); // 使用一個特殊標記
                        // 延遲一小段時間，確保訊息發送
                        Thread.Sleep(100);
                    }
                    catch { /* 發送失敗也沒關係 */ }
                }

                // 關閉連接
                if (_connectedClient != null)
                {
                    try { _connectedClient.Close(); } catch { }
                    _connectedClient = null;
                }

                // 停止接收執行緒
                if (thrReceiver != null && thrReceiver.IsAlive)
                {
                    try { thrReceiver.Join(100); } catch { }
                }

                // 顯示父視窗 (Form1)
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

                // 移除 FormClosing 事件處理程序，避免重複觸發
                this.FormClosing -= ChatForm_FormClosing;

                // 關閉當前視窗
                this.Close();
            }
        }

        // 斷開連接按鈕事件處理器
        private void btndisconnect_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("確定要斷開連接嗎？", "確認斷開", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 只有在連接仍然有效時才嘗試發送斷開訊息
                if (_connectedClient != null && _connectedClient.Connected)
                {
                    try
                    {
                        SendMessage("<DISCONNECT>"); // 使用一個特殊標記
                        // 延遲一小段時間，確保訊息發送
                        Thread.Sleep(100);
                    }
                    catch { /* 發送失敗也沒關係 */ }
                }

                // 關閉連接
                if (_connectedClient != null)
                {
                    try { _connectedClient.Close(); } catch { }
                    _connectedClient = null;
                }

                // 停止接收執行緒 (更安全的方式)
                if (thrReceiver != null && thrReceiver.IsAlive)
                {
                    // 設置一個標誌或使用 CancellationTokenSource 是更優選的方式
                    try { thrReceiver.Join(100); } catch { }
                }
                // thrReceiver = null; // 清理引用

                // 關閉當前聊天視窗
                this.Close();

                // 顯示父視窗 (Form1) 作為後備，以防 FormClosing 未能成功顯示
                if (_parentForm != null && !_parentForm.IsDisposed)
                {
                    // 在 UI 執行緒上顯示父視窗，以防當前在其他執行緒中觸發關閉
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
            }
        }

        // 處理圖片按鈕點擊事件
        private void btnpicture_Click(object sender, EventArgs e)
        {
            Pictureform pictureForm = new Pictureform(_connectedClient);
            pictureForm.Show();
        }

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
            if (btnpicture != null && !btnpicture.IsDisposed) btnpicture.Enabled = false;  // 停用圖片按鈕
            if (btnreadpic != null && !btnreadpic.IsDisposed) btnreadpic.Enabled = false;  // 停用查看圖片按鈕
        }

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