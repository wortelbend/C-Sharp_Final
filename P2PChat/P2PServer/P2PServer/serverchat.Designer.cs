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
            this.btnclear = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnpicture = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbMessages
            // 
            this.rtbMessages.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.rtbMessages.Location = new System.Drawing.Point(9, 2);
            this.rtbMessages.Name = "rtbMessages";
            this.rtbMessages.ReadOnly = true;
            this.rtbMessages.Size = new System.Drawing.Size(533, 485);
            this.rtbMessages.TabIndex = 2;
            this.rtbMessages.Text = "";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(18, 533);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(654, 47);
            this.txtMessage.TabIndex = 3;
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F);
            this.btnSend.Location = new System.Drawing.Point(697, 533);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(108, 46);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "發送";
            // 
            // txtchat1
            // 
            this.txtchat1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F);
            this.txtchat1.Location = new System.Drawing.Point(70, 494);
            this.txtchat1.Multiline = true;
            this.txtchat1.Name = "txtchat1";
            this.txtchat1.Size = new System.Drawing.Size(120, 33);
            this.txtchat1.TabIndex = 5;
            this.txtchat1.Text = "您好!";
            // 
            // btnchat1
            // 
            this.btnchat1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F);
            this.btnchat1.Location = new System.Drawing.Point(546, 492);
            this.btnchat1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnchat1.Name = "btnchat1";
            this.btnchat1.Size = new System.Drawing.Size(82, 34);
            this.btnchat1.TabIndex = 6;
            this.btnchat1.Text = "傳送";
            this.btnchat1.UseVisualStyleBackColor = true;
            // 
            // btnchat2
            // 
            this.btnchat2.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F);
            this.btnchat2.Location = new System.Drawing.Point(196, 493);
            this.btnchat2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnchat2.Name = "btnchat2";
            this.btnchat2.Size = new System.Drawing.Size(82, 34);
            this.btnchat2.TabIndex = 8;
            this.btnchat2.Text = "傳送";
            this.btnchat2.UseVisualStyleBackColor = true;
            // 
            // txtchat2
            // 
            this.txtchat2.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F);
            this.txtchat2.Location = new System.Drawing.Point(405, 493);
            this.txtchat2.Multiline = true;
            this.txtchat2.Name = "txtchat2";
            this.txtchat2.Size = new System.Drawing.Size(120, 33);
            this.txtchat2.TabIndex = 7;
            this.txtchat2.Text = "再見";
            // 
            // btnchat3
            // 
            this.btnchat3.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F);
            this.btnchat3.Location = new System.Drawing.Point(811, 491);
            this.btnchat3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnchat3.Name = "btnchat3";
            this.btnchat3.Size = new System.Drawing.Size(100, 33);
            this.btnchat3.TabIndex = 10;
            this.btnchat3.Text = "傳送";
            this.btnchat3.UseVisualStyleBackColor = true;
            // 
            // txtchat3
            // 
            this.txtchat3.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F);
            this.txtchat3.Location = new System.Drawing.Point(685, 492);
            this.txtchat3.Multiline = true;
            this.txtchat3.Name = "txtchat3";
            this.txtchat3.Size = new System.Drawing.Size(120, 32);
            this.txtchat3.TabIndex = 9;
            this.txtchat3.Text = "(表情)";
            // 
            // btndisconnect
            // 
            this.btndisconnect.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F);
            this.btndisconnect.Location = new System.Drawing.Point(457, 584);
            this.btndisconnect.Name = "btndisconnect";
            this.btndisconnect.Size = new System.Drawing.Size(491, 35);
            this.btndisconnect.TabIndex = 11;
            this.btndisconnect.Text = "斷開連線";
            // 
            // btnclear
            // 
            this.btnclear.Font = new System.Drawing.Font("Microsoft JhengHei UI", 11F);
            this.btnclear.Location = new System.Drawing.Point(12, 582);
            this.btnclear.Name = "btnclear";
            this.btnclear.Size = new System.Drawing.Size(299, 37);
            this.btnclear.TabIndex = 4;
            this.btnclear.Text = "清除聊天室";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.pictureBox1.Location = new System.Drawing.Point(548, 2);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(411, 262);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F);
            this.label1.Location = new System.Drawing.Point(680, 266);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 28);
            this.label1.TabIndex = 13;
            this.label1.Text = "對方傳送的圖片";
            // 
            // btnpicture
            // 
            this.btnpicture.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F);
            this.btnpicture.Location = new System.Drawing.Point(830, 533);
            this.btnpicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnpicture.Name = "btnpicture";
            this.btnpicture.Size = new System.Drawing.Size(118, 46);
            this.btnpicture.TabIndex = 14;
            this.btnpicture.Text = "傳送圖片";
            this.btnpicture.UseVisualStyleBackColor = true;
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 631);
            this.Controls.Add(this.btnpicture);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btndisconnect);
            this.Controls.Add(this.btnchat3);
            this.Controls.Add(this.txtchat3);
            this.Controls.Add(this.btnchat2);
            this.Controls.Add(this.txtchat2);
            this.Controls.Add(this.btnchat1);
            this.Controls.Add(this.txtchat1);
            this.Controls.Add(this.rtbMessages);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnclear);
            this.Controls.Add(this.btnSend);
            this.Name = "ChatForm";
            this.Text = "P2P 聊天(伺服器端)";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

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
        private Button btnclear;
        private PictureBox pictureBox1;
        private Label label1;
        private Button btnpicture;
    }
}