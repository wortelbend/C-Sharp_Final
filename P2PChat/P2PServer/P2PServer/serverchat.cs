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
            // 為發送按鈕和文本框的 Enter 事件添加處理器
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            this.txtMessage.KeyDown += new KeyEventHandler(this.txtMessage_KeyDown);

            // 假設存在 btnchat1, btnchat2, btnchat3 和 txtchat1, txtchat2, txtchat3 並添加事件處理器
            // 請根據您的實際控制項名稱和類型進行調整或確認
            // 例如：this.btnchat1.Click += new System.EventHandler(this.btnchat1_Click);
            //       this.btnchat2.Click += new System.EventHandler(this.btnchat2_Click);
            //       this.btnchat3.Click += new System.EventHandler(this.btnchat3_Click);

            // 添加 btnchat1, btnchat2, btnchat3 的事件處理器
            if (btnchat1 != null) this.btnchat1.Click += new System.EventHandler(this.btnchat_Click);
            if (btnchat2 != null) this.btnchat2.Click += new System.EventHandler(this.btnchat_Click);
            if (btnchat3 != null) this.btnchat3.Click += new System.EventHandler(this.btnchat_Click);
        }

        // 新增一個建構子，接收一個已連接的 TcpClient 物件
        public ChatForm(TcpClient connectedClient)
        {
            InitializeComponent();
            _connectedClient = connectedClient;

            // 禁用監聽和連接按鈕，因為連線已建立
            if (btnListen != null) btnListen.Enabled = false;
            if (btnConnect != null) btnConnect.Enabled = false;
            // 禁用 IP/Port 輸入框
            if (txtLocalIP != null) txtLocalIP.Enabled = false;
            if (txtLocalPort != null) txtLocalPort.Enabled = false;
            if (txtRemoteIP != null) txtRemoteIP.Enabled = false;
            if (txtRemotePort != null) txtRemotePort.Enabled = false;

            // 啟動接收訊息的執行緒
            thrReceiver = new Thread(new ThreadStart(ReceiverThread));
            thrReceiver.IsBackground = true; // 設定為背景執行緒
            thrReceiver.Start();

            AppendMessage("連線成功！");

            // 為發送按鈕和文本框的 Enter 事件添加處理器
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            this.txtMessage.KeyDown += new KeyEventHandler(this.txtMessage_KeyDown);

            // 為斷開連接按鈕添加事件處理器
            if (btndisconnect != null) this.btndisconnect.Click += new System.EventHandler(this.btndisconnect_Click);

            // 添加 btnchat1, btnchat2, btnchat3 的事件處理器
            if (btnchat1 != null) this.btnchat1.Click += new System.EventHandler(this.btnchat_Click);
            if (btnchat2 != null) this.btnchat2.Click += new System.EventHandler(this.btnchat_Click);
            if (btnchat3 != null) this.btnchat3.Click += new System.EventHandler(this.btnchat_Click);
        }

        // 新增一個建構子，接收一個已連接的 TcpClient 物件和父視窗參考
        public ChatForm(TcpClient connectedClient, Form parentForm)
        {
            InitializeComponent();
            _connectedClient = connectedClient;
            _parentForm = parentForm; // 儲存父視窗參考

            // 禁用監聽和連接按鈕，因為連線已建立
            if (btnListen != null) btnListen.Enabled = false;
            if (btnConnect != null) btnConnect.Enabled = false;
            // 禁用 IP/Port 輸入框
            if (txtLocalIP != null) txtLocalIP.Enabled = false;
            if (txtLocalPort != null) txtLocalPort.Enabled = false;
            if (txtRemoteIP != null) txtRemoteIP.Enabled = false;
            if (txtRemotePort != null) txtRemotePort.Enabled = false;

            // 啟動接收訊息的執行緒
            thrReceiver = new Thread(new ThreadStart(ReceiverThread));
            thrReceiver.IsBackground = true; // 設定為背景執行緒
            thrReceiver.Start();

            AppendMessage("連線成功！");

            // 為發送按鈕和文本框的 Enter 事件添加處理器
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            this.txtMessage.KeyDown += new KeyEventHandler(this.txtMessage_KeyDown);

            // 為斷開連接按鈕添加事件處理器
            if (btndisconnect != null) this.btndisconnect.Click += new System.EventHandler(this.btndisconnect_Click);

            // 添加 btnchat1, btnchat2, btnchat3 的事件處理器
            if (btnchat1 != null) this.btnchat1.Click += new System.EventHandler(this.btnchat_Click);
            if (btnchat2 != null) this.btnchat2.Click += new System.EventHandler(this.btnchat_Click);
            if (btnchat3 != null) this.btnchat3.Click += new System.EventHandler(this.btnchat_Click);
        }

        private void InitializeNetwork()
        {
            // 獲取本機網路資訊
            strHostname = Dns.GetHostName();
            ipaLocal = Dns.Resolve(strHostname).AddressList[0];
            strLocalIP = ipaLocal.ToString();
            txtLocalIP.Text = strLocalIP;
            txtLocalPort.Text = localPort.ToString();
            txtRemoteIP.Text = strLocalIP;
            txtRemotePort.Text = remotePort.ToString();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            MessageBox.Show("連線已建立，無需再次監聽", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            MessageBox.Show("連線已建立，無需再次連接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        // 創建一個通用的按鈕點擊事件處理器
        private void btnchat_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string predefinedMessage = "";

            if (clickedButton != null)
            {
                // 根據按鈕的名稱獲取對應的文本框內容
                switch (clickedButton.Name)
                {
                    case "btnchat1":
                        if (txtchat1 != null) predefinedMessage = txtchat1.Text;
                        break;
                    case "btnchat2":
                        if (txtchat2 != null) predefinedMessage = txtchat2.Text;
                        break;
                    case "btnchat3":
                        if (txtchat3 != null) predefinedMessage = txtchat3.Text;
                        break;
                }

                // 如果獲取到了預設文字且連接有效，則發送訊息
                if (!string.IsNullOrEmpty(predefinedMessage))
                {
                    // 使用傳入的 _connectedClient 來發送訊息
                    try
                    {
                        byte[] data = Encoding.Unicode.GetBytes(predefinedMessage);
                        if (_connectedClient != null && _connectedClient.Connected)
                        {
                            _connectedClient.GetStream().Write(data, 0, data.Length);
                            AppendMessage("我: " + predefinedMessage); // 發送方自己顯示訊息
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
                else
                {
                    MessageBox.Show("預設文字內容為空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // 修改 SendMessage 方法，使其可以選擇性地接受一個字串參數
        private void SendMessage(string messageToSend = null)
        {
            string message = messageToSend ?? txtMessage.Text; // 如果傳入了 messageToSend，則使用它，否則使用 txtMessage.Text
            if (string.IsNullOrEmpty(message)) return;

            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message);

                // 檢查是否使用傳入的 TcpClient
                if (_connectedClient != null && _connectedClient.Connected)
                {
                    _connectedClient.GetStream().Write(data, 0, data.Length); // 使用 GetStream() 獲取 NetworkStream
                    AppendMessage("我: " + message); // 發送方自己顯示訊息
                    txtMessage.Clear();
                }
                // 保留原有的 Socket 邏輯作為後備 (如果沒有使用帶 TcpClient 的建構子)
                else if (isConnected) // 客戶端模式 (雖然在 serverchat 中，但如果作為客戶端連線時使用)
                {
                    sktConnect.Send(data);
                    AppendMessage("我: " + message); // 發送方自己顯示訊息
                    txtMessage.Clear();
                }
                else if (sktClient != null && sktClient.Connected) // 伺服器模式（接受單個客戶端）
                {
                    sktClient.Send(data);
                    AppendMessage("我: " + message); // 發送方自己顯示訊息
                    txtMessage.Clear();
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

        private void ReceiverThread()
        {
            // 這個訊息可能在連接斷開後無法顯示，但保留用於調試
            // AppendMessage("接收執行緒啟動失敗：未建立連接");
            if (_connectedClient == null || !_connectedClient.Connected)
            {
                // 如果一開始就沒有連接，執行緒可以安全退出
                return;
            }

            NetworkStream stream = _connectedClient.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                // 只有當連接有效時才繼續接收。視窗關閉會由 FormClosing 處理
                while (_connectedClient.Connected)
                {
                    int received = stream.Read(buffer, 0, buffer.Length);
                    if (received > 0)
                    {
                        string message = Encoding.Unicode.GetString(buffer, 0, received);
                        // 檢查是否收到斷開連接訊息
                        if (message == "<DISCONNECT>")
                        {
                            AppendMessage("對方已經斷開連接");
                            // 收到斷開訊息後，關閉本地連接
                            if (_connectedClient != null)
                            {
                                try { _connectedClient.Close(); } catch { }
                                _connectedClient = null;
                            }
                            // 禁用發送功能，停留在頁面
                            this.Invoke((MethodInvoker)delegate {
                                if (txtMessage != null && !txtMessage.IsDisposed) txtMessage.Enabled = false;
                                if (btnSend != null && !btnSend.IsDisposed) btnSend.Enabled = false;
                                // 禁用預設訊息按鈕
                                if (btnchat1 != null && !btnchat1.IsDisposed) btnchat1.Enabled = false;
                                if (btnchat2 != null && !btnchat2.IsDisposed) btnchat2.Enabled = false;
                                if (btnchat3 != null && !btnchat3.IsDisposed) btnchat3.Enabled = false;
                            });
                            break; // 退出接收循環
                        }
                        else
                        {
                            AppendMessage("對方: " + message);
                        }
                    }
                    else if (received == 0) // 連接已正常關閉 (對端關閉或網路問題)
                    {
                        // 如果 received == 0 且沒有收到 <DISCONNECT>，也視為對方斷開
                        AppendMessage("連接已斷開 (對方關閉)");
                        // 關閉本地連接
                        if (_connectedClient != null)
                        {
                            try { _connectedClient.Close(); } catch { }
                            _connectedClient = null;
                        }
                        // 禁用發送功能
                        // 在 Invoke 外層添加 try-catch 和 IsHandleCreated 檢查
                        if (this.IsHandleCreated && !this.IsDisposed)
                        {
                            try
                            {
                                this.Invoke((MethodInvoker)delegate {
                                    if (!this.IsDisposed && !this.Disposing && txtMessage != null && btnSend != null)
                                    {
                                        txtMessage.Enabled = false;
                                        btnSend.Enabled = false;
                                    }
                                    // 禁用預設訊息按鈕
                                    if (btnchat1 != null && !btnchat1.IsDisposed) btnchat1.Enabled = false;
                                    if (btnchat2 != null && !btnchat2.IsDisposed) btnchat2.Enabled = false;
                                    if (btnchat3 != null && !btnchat3.IsDisposed) btnchat3.Enabled = false;
                                });
                            }
                            catch (ObjectDisposedException) { /* 忽略 Form 處置時的異常 */ }
                            catch (InvalidOperationException) { /* 忽略 Form 句柄無效時的異常 */ }
                        }

                        break; // 退出接收循環
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // 連接可能在讀取時被關閉，這是正常的
                // AppendMessage("接收執行緒：連接已關閉。"); // 在這裡不更新 UI，避免新的異常
            }
            catch (InvalidOperationException ex)
            {
                // 在 Invoke 呼叫期間表單可能已被處置或句柄無效
                // AppendMessage($"接收執行緒：無法在無效的控制項上呼叫 Invoke。錯誤: {ex.Message}"); // 在這裡不更新 UI
            }
            catch (Exception ex)
            {
                // 接收發生錯誤，例如連接中斷
                if (_connectedClient != null && _connectedClient.Connected) // 如果連接仍然是活的，可能是其他錯誤
                {
                    AppendMessage("接收錯誤: " + ex.Message);
                }
                else // 連接已斷開 (可能由對端關閉或網路問題導致)
                {
                    AppendMessage("連接已斷開或發生錯誤");
                }

                // 發生錯誤時也關閉連接並禁用發送功能，停留在頁面
                if (_connectedClient != null)
                {
                    try { _connectedClient.Close(); } catch { }
                    _connectedClient = null;
                }
                // 在 Invoke 外層添加 try-catch 和 IsHandleCreated 檢查
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate {
                            if (!this.IsDisposed && !this.Disposing && txtMessage != null && btnSend != null)
                            {
                                txtMessage.Enabled = false;
                                btnSend.Enabled = false;
                            }
                            // 禁用預設訊息按鈕
                            if (btnchat1 != null && !btnchat1.IsDisposed) btnchat1.Enabled = false;
                            if (btnchat2 != null && !btnchat2.IsDisposed) btnchat2.Enabled = false;
                            if (btnchat3 != null && !btnchat3.IsDisposed) btnchat3.Enabled = false;
                        });
                    }
                    catch (ObjectDisposedException) { /* 忽略 Form 處置時的異常 */ }
                    catch (InvalidOperationException) { /* 忽略 Form 句柄無效時的異常 */ }
                }
            }
            finally
            {
                // 接收執行緒結束時，清理相關資源
                // 連接已在上面關閉
                // AppendMessage("接收執行緒結束。"); // 在非活動連接或 Form 關閉時可能無法顯示
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

        private void AppendMessage(string message)
        {
            // 在嘗試 Invoke 或更新 UI 之前，檢查 RichTextBox 是否已處置或正在處置，並檢查句柄是否已建立
            if (rtbMessages == null || rtbMessages.IsDisposed || this.IsDisposed || this.Disposing || !rtbMessages.IsHandleCreated) return;

            if (rtbMessages.InvokeRequired)
            {
                try
                {
                    // 在 Invoke 的委託中再次檢查，確保在執行時控制項仍然有效且句柄存在
                    rtbMessages.Invoke((MethodInvoker)delegate
                    {
                        if (rtbMessages != null && !rtbMessages.IsDisposed && !this.IsDisposed && !this.Disposing && rtbMessages.IsHandleCreated)
                        {
                            rtbMessages.AppendText(message + Environment.NewLine);
                        }
                    });
                }
                catch (ObjectDisposedException) { /* 忽略在控制項處置時發生的異常 */ }
                catch (InvalidOperationException) { /* 忽略在 Form 關閉時可能發生的異常 */ }
            }
            else
            {
                // 在 UI 執行緒中直接更新，但仍需檢查狀態
                if (rtbMessages != null && !rtbMessages.IsDisposed && !this.IsDisposed && !this.Disposing && rtbMessages.IsHandleCreated)
                {
                    rtbMessages.AppendText(message + Environment.NewLine);
                }
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // StopListening(); // 移除或不執行內部的停止監聽邏輯
            // DisconnectClient(); // 移除或不執行內部的斷開邏輯

            // 向對方發送斷開連接訊息 (只有在連接仍然有效時)
            // 注意：如果在 ReceiverThread 中因為收到 DISCONNECT 而觸發的 Close，這裡可能無需再次發送
            // 為了安全起見，可以在關閉前嘗試發送，但要處理可能發生的異常
            if (_connectedClient != null && _connectedClient.Connected)
            {
                try
                {
                    // 使用 SendMessage 方法發送，它已經包含連接檢查和異常處理
                    SendMessage("<DISCONNECT>");
                    // 延遲一小段時間確保訊息發送
                    Thread.Sleep(100);
                }
                catch { /* 發送失敗也沒關係 */ }
            }

            // 安全關閉連接
            if (_connectedClient != null)
            {
                try
                {
                    _connectedClient.Close();
                }
                catch { }
                _connectedClient = null;
            }

            // 等待接收執行緒結束
            // 設定 thrReceiver 為背景執行緒後，應用程式關閉時會自動終止，但最好能等待它乾淨退出
            if (thrReceiver != null && thrReceiver.IsAlive)
            {
                // 嘗試等待執行緒自然結束一段時間
                if (!thrReceiver.Join(500)) // 等待最多 500 毫秒
                {
                    // 如果執行緒仍未結束，考慮更溫和的中止方式或依賴背景執行緒自動終止
                    // 這裡不使用 Abort，依賴背景執行緒屬性或讓它自然退出
                    // AppendMessage("接收執行緒未能及時結束。"); // 在 FormClosing 中不應更新 UI
                }
            }
            // thrReceiver = null; // 清理引用

            // 如果 ChatForm 關閉，且父視窗存在但未顯示，則顯示父視窗
            // 這段邏輯放在這裡確保 ChatForm 關閉後才顯示父視窗
            if (_parentForm != null && !_parentForm.Visible)
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

        // 斷開連接按鈕事件處理器
        private void btndisconnect_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("確定要斷開連接嗎？", "確認斷開", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 如果連接仍然有效，則向對方發送斷開連接訊息
                // 在客戶端已斷開的情況下，這裡不會執行發送
                if (_connectedClient != null && _connectedClient.Connected)
                {
                    try
                    {
                        SendMessage("<DISCONNECT>"); // 使用一個特殊標記
                        // 延遲一小段時間，確保斷開訊息發送出去
                        Thread.Sleep(100);
                    }
                    catch { /* 發送失敗也沒關係 */ }
                }

                // 關閉連接，無論是否成功發送了斷開訊息
                if (_connectedClient != null)
                {
                    try { _connectedClient.Close(); } catch { }
                    _connectedClient = null;
                }

                // 停止接收執行緒 (更安全的方式)
                if (thrReceiver != null && thrReceiver.IsAlive)
                {
                    // 設置一個標誌或使用 CancellationTokenSource 是更優選的方式
                    // 如果沒有這樣的機制，可以嘗試 Join 短暫時間，或僅依賴背景執行緒屬性
                    // 這裡暫時不使用 Abort()
                    try { thrReceiver.Join(100); } catch { }
                    // 如果執行緒仍然存活，考慮更溫和的處理，或依賴背景執行緒在應用程式關閉時終止
                }
                // thrReceiver = null; // 清理引用

                // 關閉當前聊天視窗
                this.Close();

                // 顯示父視窗 (serverForm1)
                // FormClosing 事件已經處理了顯示父視窗的邏輯，這裡可以移除重複的呼叫
                // if (_parentForm != null)
                // {
                //     _parentForm.Show();
                // }
                // 重新啟用顯示父視窗的邏輯作為後備
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
    }
}