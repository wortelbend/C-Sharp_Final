using System.Drawing;
using System.Windows.Forms;

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
            this.grpLocal = new System.Windows.Forms.GroupBox();
            this.lblLocalIP = new System.Windows.Forms.Label();
            this.txtLocalIP = new System.Windows.Forms.TextBox();
            this.lblLocalPort = new System.Windows.Forms.Label();
            this.txtLocalPort = new System.Windows.Forms.TextBox();
            this.btnListen = new System.Windows.Forms.Button();
            this.grpRemote = new System.Windows.Forms.GroupBox();
            this.lblRemoteIP = new System.Windows.Forms.Label();
            this.txtRemoteIP = new System.Windows.Forms.TextBox();
            this.lblRemotePort = new System.Windows.Forms.Label();
            this.txtRemotePort = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.rtbMessages = new System.Windows.Forms.RichTextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtchat1 = new System.Windows.Forms.TextBox();
            this.btnchat1 = new System.Windows.Forms.Button();
            this.btnchat2 = new System.Windows.Forms.Button();
            this.txtchat2 = new System.Windows.Forms.TextBox();
            this.btnchat3 = new System.Windows.Forms.Button();
            this.txtchat3 = new System.Windows.Forms.TextBox();
            this.btndisconnect = new System.Windows.Forms.Button();
            this.grpLocal.SuspendLayout();
            this.grpRemote.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpLocal
            // 
            this.grpLocal.Controls.Add(this.lblLocalIP);
            this.grpLocal.Controls.Add(this.txtLocalIP);
            this.grpLocal.Controls.Add(this.lblLocalPort);
            this.grpLocal.Controls.Add(this.txtLocalPort);
            this.grpLocal.Controls.Add(this.btnListen);
            this.grpLocal.Location = new System.Drawing.Point(117, 10);
            this.grpLocal.Name = "grpLocal";
            this.grpLocal.Size = new System.Drawing.Size(270, 100);
            this.grpLocal.TabIndex = 0;
            this.grpLocal.TabStop = false;
            this.grpLocal.Text = "本地設置";
            // 
            // lblLocalIP
            // 
            this.lblLocalIP.AutoSize = true;
            this.lblLocalIP.Location = new System.Drawing.Point(6, 20);
            this.lblLocalIP.Name = "lblLocalIP";
            this.lblLocalIP.Size = new System.Drawing.Size(45, 12);
            this.lblLocalIP.TabIndex = 0;
            this.lblLocalIP.Text = "本地 IP:";
            // 
            // txtLocalIP
            // 
            this.txtLocalIP.Location = new System.Drawing.Point(70, 17);
            this.txtLocalIP.Name = "txtLocalIP";
            this.txtLocalIP.ReadOnly = true;
            this.txtLocalIP.Size = new System.Drawing.Size(120, 22);
            this.txtLocalIP.TabIndex = 1;
            // 
            // lblLocalPort
            // 
            this.lblLocalPort.AutoSize = true;
            this.lblLocalPort.Location = new System.Drawing.Point(6, 50);
            this.lblLocalPort.Name = "lblLocalPort";
            this.lblLocalPort.Size = new System.Drawing.Size(56, 12);
            this.lblLocalPort.TabIndex = 2;
            this.lblLocalPort.Text = "本地端口:";
            // 
            // txtLocalPort
            // 
            this.txtLocalPort.Location = new System.Drawing.Point(70, 47);
            this.txtLocalPort.Name = "txtLocalPort";
            this.txtLocalPort.Size = new System.Drawing.Size(120, 22);
            this.txtLocalPort.TabIndex = 3;
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(196, 17);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(68, 52);
            this.btnListen.TabIndex = 4;
            this.btnListen.Text = "開始監聽";
            // 
            // grpRemote
            // 
            this.grpRemote.Controls.Add(this.lblRemoteIP);
            this.grpRemote.Controls.Add(this.txtRemoteIP);
            this.grpRemote.Controls.Add(this.lblRemotePort);
            this.grpRemote.Controls.Add(this.txtRemotePort);
            this.grpRemote.Controls.Add(this.btnConnect);
            this.grpRemote.Location = new System.Drawing.Point(407, 10);
            this.grpRemote.Name = "grpRemote";
            this.grpRemote.Size = new System.Drawing.Size(270, 100);
            this.grpRemote.TabIndex = 1;
            this.grpRemote.TabStop = false;
            this.grpRemote.Text = "遠端設置";
            // 
            // lblRemoteIP
            // 
            this.lblRemoteIP.AutoSize = true;
            this.lblRemoteIP.Location = new System.Drawing.Point(6, 20);
            this.lblRemoteIP.Name = "lblRemoteIP";
            this.lblRemoteIP.Size = new System.Drawing.Size(45, 12);
            this.lblRemoteIP.TabIndex = 0;
            this.lblRemoteIP.Text = "遠端 IP:";
            // 
            // txtRemoteIP
            // 
            this.txtRemoteIP.Location = new System.Drawing.Point(69, 17);
            this.txtRemoteIP.Name = "txtRemoteIP";
            this.txtRemoteIP.Size = new System.Drawing.Size(120, 22);
            this.txtRemoteIP.TabIndex = 1;
            // 
            // lblRemotePort
            // 
            this.lblRemotePort.AutoSize = true;
            this.lblRemotePort.Location = new System.Drawing.Point(6, 50);
            this.lblRemotePort.Name = "lblRemotePort";
            this.lblRemotePort.Size = new System.Drawing.Size(56, 12);
            this.lblRemotePort.TabIndex = 2;
            this.lblRemotePort.Text = "遠端端口:";
            // 
            // txtRemotePort
            // 
            this.txtRemotePort.Location = new System.Drawing.Point(70, 47);
            this.txtRemotePort.Name = "txtRemotePort";
            this.txtRemotePort.Size = new System.Drawing.Size(120, 22);
            this.txtRemotePort.TabIndex = 3;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(196, 17);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(68, 52);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "連接";
            // 
            // rtbMessages
            // 
            this.rtbMessages.Location = new System.Drawing.Point(27, 177);
            this.rtbMessages.Name = "rtbMessages";
            this.rtbMessages.ReadOnly = true;
            this.rtbMessages.Size = new System.Drawing.Size(761, 236);
            this.rtbMessages.TabIndex = 2;
            this.rtbMessages.Text = "";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(27, 418);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(595, 47);
            this.txtMessage.TabIndex = 3;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(641, 418);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(90, 46);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "發送";
            // 
            // txtchat1
            // 
            this.txtchat1.Location = new System.Drawing.Point(27, 119);
            this.txtchat1.Name = "txtchat1";
            this.txtchat1.Size = new System.Drawing.Size(120, 22);
            this.txtchat1.TabIndex = 5;
            this.txtchat1.Text = "您好!";
            // 
            // btnchat1
            // 
            this.btnchat1.Location = new System.Drawing.Point(152, 118);
            this.btnchat1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnchat1.Name = "btnchat1";
            this.btnchat1.Size = new System.Drawing.Size(82, 20);
            this.btnchat1.TabIndex = 6;
            this.btnchat1.Text = "傳送";
            this.btnchat1.UseVisualStyleBackColor = true;
            // 
            // btnchat2
            // 
            this.btnchat2.Location = new System.Drawing.Point(426, 119);
            this.btnchat2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnchat2.Name = "btnchat2";
            this.btnchat2.Size = new System.Drawing.Size(64, 18);
            this.btnchat2.TabIndex = 8;
            this.btnchat2.Text = "傳送";
            this.btnchat2.UseVisualStyleBackColor = true;
            // 
            // txtchat2
            // 
            this.txtchat2.Location = new System.Drawing.Point(301, 120);
            this.txtchat2.Name = "txtchat2";
            this.txtchat2.Size = new System.Drawing.Size(120, 22);
            this.txtchat2.TabIndex = 7;
            this.txtchat2.Text = "再見";
            // 
            // btnchat3
            // 
            this.btnchat3.Location = new System.Drawing.Point(737, 118);
            this.btnchat3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnchat3.Name = "btnchat3";
            this.btnchat3.Size = new System.Drawing.Size(50, 18);
            this.btnchat3.TabIndex = 10;
            this.btnchat3.Text = "傳送";
            this.btnchat3.UseVisualStyleBackColor = true;
            // 
            // txtchat3
            // 
            this.txtchat3.Location = new System.Drawing.Point(612, 119);
            this.txtchat3.Name = "txtchat3";
            this.txtchat3.Size = new System.Drawing.Size(120, 22);
            this.txtchat3.TabIndex = 9;
            this.txtchat3.Text = "(表情)";
            // 
            // btndisconnect
            // 
            this.btndisconnect.Location = new System.Drawing.Point(738, 418);
            this.btndisconnect.Name = "btndisconnect";
            this.btndisconnect.Size = new System.Drawing.Size(90, 46);
            this.btndisconnect.TabIndex = 11;
            this.btndisconnect.Text = "斷開連線";
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 505);
            this.Controls.Add(this.btndisconnect);
            this.Controls.Add(this.btnchat3);
            this.Controls.Add(this.txtchat3);
            this.Controls.Add(this.btnchat2);
            this.Controls.Add(this.txtchat2);
            this.Controls.Add(this.btnchat1);
            this.Controls.Add(this.txtchat1);
            this.Controls.Add(this.grpLocal);
            this.Controls.Add(this.grpRemote);
            this.Controls.Add(this.rtbMessages);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            this.Name = "ChatForm";
            this.Text = "P2P 聊天";
            this.grpLocal.ResumeLayout(false);
            this.grpLocal.PerformLayout();
            this.grpRemote.ResumeLayout(false);
            this.grpRemote.PerformLayout();
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
        private TextBox txtchat1;
        private Button btnchat1;
        private Button btnchat2;
        private TextBox txtchat2;
        private Button btnchat3;
        private TextBox txtchat3;
        private Button btndisconnect;
    }
}