namespace P2PChat
{
    partial class ChatForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            
            // 本地設置群組
            this.grpLocal = new System.Windows.Forms.GroupBox();
            this.lblLocalIP = new System.Windows.Forms.Label();
            this.txtLocalIP = new System.Windows.Forms.TextBox();
            this.lblLocalPort = new System.Windows.Forms.Label();
            this.txtLocalPort = new System.Windows.Forms.TextBox();
            this.btnListen = new System.Windows.Forms.Button();
            
            // 遠端設置群組
            this.grpRemote = new System.Windows.Forms.GroupBox();
            this.lblRemoteIP = new System.Windows.Forms.Label();
            this.txtRemoteIP = new System.Windows.Forms.TextBox();
            this.lblRemotePort = new System.Windows.Forms.Label();
            this.txtRemotePort = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            
            // 聊天區域
            this.rtbMessages = new System.Windows.Forms.RichTextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();

            // 設置表單
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Text = "P2P 聊天";
            
            // 設置本地群組
            this.grpLocal.Text = "本地設置";
            this.grpLocal.Location = new System.Drawing.Point(12, 12);
            this.grpLocal.Size = new System.Drawing.Size(270, 100);
            
            this.lblLocalIP.Text = "本地 IP:";
            this.lblLocalIP.Location = new System.Drawing.Point(6, 20);
            this.lblLocalIP.AutoSize = true;
            
            this.txtLocalIP.Location = new System.Drawing.Point(70, 17);
            this.txtLocalIP.Size = new System.Drawing.Size(120, 22);
            this.txtLocalIP.ReadOnly = true;
            
            this.lblLocalPort.Text = "本地端口:";
            this.lblLocalPort.Location = new System.Drawing.Point(6, 50);
            this.lblLocalPort.AutoSize = true;
            
            this.txtLocalPort.Location = new System.Drawing.Point(70, 47);
            this.txtLocalPort.Size = new System.Drawing.Size(120, 22);
            
            this.btnListen.Text = "開始監聽";
            this.btnListen.Location = new System.Drawing.Point(196, 17);
            this.btnListen.Size = new System.Drawing.Size(68, 52);
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            
            // 設置遠端群組
            this.grpRemote.Text = "遠端設置";
            this.grpRemote.Location = new System.Drawing.Point(302, 12);
            this.grpRemote.Size = new System.Drawing.Size(270, 100);
            
            this.lblRemoteIP.Text = "遠端 IP:";
            this.lblRemoteIP.Location = new System.Drawing.Point(6, 20);
            this.lblRemoteIP.AutoSize = true;
            
            this.txtRemoteIP.Location = new System.Drawing.Point(70, 17);
            this.txtRemoteIP.Size = new System.Drawing.Size(120, 22);
            
            this.lblRemotePort.Text = "遠端端口:";
            this.lblRemotePort.Location = new System.Drawing.Point(6, 50);
            this.lblRemotePort.AutoSize = true;
            
            this.txtRemotePort.Location = new System.Drawing.Point(70, 47);
            this.txtRemotePort.Size = new System.Drawing.Size(120, 22);
            
            this.btnConnect.Text = "連接";
            this.btnConnect.Location = new System.Drawing.Point(196, 17);
            this.btnConnect.Size = new System.Drawing.Size(68, 52);
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            
            // 設置聊天區域
            this.rtbMessages.Location = new System.Drawing.Point(12, 118);
            this.rtbMessages.Size = new System.Drawing.Size(560, 280);
            this.rtbMessages.ReadOnly = true;
            
            this.txtMessage.Location = new System.Drawing.Point(12, 404);
            this.txtMessage.Size = new System.Drawing.Size(479, 22);
            
            this.btnSend.Text = "發送";
            this.btnSend.Location = new System.Drawing.Point(497, 404);
            this.btnSend.Size = new System.Drawing.Size(75, 22);
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            
            // 添加控制項到群組
            this.grpLocal.Controls.Add(this.lblLocalIP);
            this.grpLocal.Controls.Add(this.txtLocalIP);
            this.grpLocal.Controls.Add(this.lblLocalPort);
            this.grpLocal.Controls.Add(this.txtLocalPort);
            this.grpLocal.Controls.Add(this.btnListen);
            
            this.grpRemote.Controls.Add(this.lblRemoteIP);
            this.grpRemote.Controls.Add(this.txtRemoteIP);
            this.grpRemote.Controls.Add(this.lblRemotePort);
            this.grpRemote.Controls.Add(this.txtRemotePort);
            this.grpRemote.Controls.Add(this.btnConnect);
            
            // 添加控制項到表單
            this.Controls.Add(this.grpLocal);
            this.Controls.Add(this.grpRemote);
            this.Controls.Add(this.rtbMessages);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.GroupBox grpLocal;
        private System.Windows.Forms.Label lblLocalIP;
        private System.Windows.Forms.TextBox txtLocalIP;
        private System.Windows.Forms.Label lblLocalPort;
        private System.Windows.Forms.TextBox txtLocalPort;
        private System.Windows.Forms.Button btnListen;
        
        private System.Windows.Forms.GroupBox grpRemote;
        private System.Windows.Forms.Label lblRemoteIP;
        private System.Windows.Forms.TextBox txtRemoteIP;
        private System.Windows.Forms.Label lblRemotePort;
        private System.Windows.Forms.TextBox txtRemotePort;
        private System.Windows.Forms.Button btnConnect;
        
        private System.Windows.Forms.RichTextBox rtbMessages;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
    }
} 