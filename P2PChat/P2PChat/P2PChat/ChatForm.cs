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

        public ChatForm()
        {
            InitializeComponent();
            InitializeNetwork();
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
            if (!isListening)
            {
                try
                {
                    localPort = int.Parse(txtLocalPort.Text);
                    IPEndPoint localEndPoint = new IPEndPoint(ipaLocal, localPort);
                    
                    sktListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sktListener.Bind(localEndPoint);
                    sktListener.Listen(1);
                    
                    thrListener = new Thread(new ThreadStart(ListenerThread));
                    thrListener.Start();
                    
                    isListening = true;
                    btnListen.Text = "停止監聽";
                    AppendMessage("開始監聽在端口: " + localPort);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("監聽失敗: " + ex.Message);
                }
            }
            else
            {
                StopListening();
                btnListen.Text = "開始監聽";
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                try
                {
                    remotePort = int.Parse(txtRemotePort.Text);
                    IPEndPoint remoteEndPoint = new IPEndPoint(
                        IPAddress.Parse(txtRemoteIP.Text), remotePort);
                    
                    sktConnect = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sktConnect.Connect(remoteEndPoint);
                    
                    thrReceiver = new Thread(new ThreadStart(ReceiverThread));
                    thrReceiver.Start();
                    
                    isConnected = true;
                    btnConnect.Text = "斷開連接";
                    AppendMessage("已連接到: " + txtRemoteIP.Text + ":" + remotePort);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("連接失敗: " + ex.Message);
                }
            }
            else
            {
                DisconnectClient();
                btnConnect.Text = "連接";
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMessage.Text)) return;
            
            try
            {
                string message = txtMessage.Text;
                byte[] data = Encoding.Unicode.GetBytes(message);
                
                if (isConnected)
                {
                    sktConnect.Send(data);
                    AppendMessage("我: " + message);
                }
                else if (sktClient != null && sktClient.Connected)
                {
                    sktClient.Send(data);
                    AppendMessage("我: " + message);
                }
                
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("發送失敗: " + ex.Message);
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
            Socket currentSocket = isConnected ? sktConnect : sktClient;
            byte[] buffer = new byte[1024];
            
            try
            {
                while (true)
                {
                    int received = currentSocket.Receive(buffer);
                    if (received > 0)
                    {
                        string message = Encoding.Unicode.GetString(buffer, 0, received);
                        AppendMessage("對方: " + message);
                    }
                }
            }
            catch
            {
                AppendMessage("連接已斷開");
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
            if (thrReceiver != null)
            {
                thrReceiver.Abort();
                thrReceiver = null;
            }
            AppendMessage("已斷開連接");
        }

        private void AppendMessage(string message)
        {
            if (rtbMessages.InvokeRequired)
            {
                rtbMessages.Invoke(new Action<string>(AppendMessage), message);
            }
            else
            {
                rtbMessages.AppendText(message + Environment.NewLine);
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopListening();
            DisconnectClient();
        }
    }
} 