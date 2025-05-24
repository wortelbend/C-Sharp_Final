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
            rtbMessages = new RichTextBox();
            txtMessage = new TextBox();
            btnSend = new Button();
            txtchat1 = new TextBox();
            btnchat1 = new Button();
            btnchat2 = new Button();
            txtchat2 = new TextBox();
            btnchat3 = new Button();
            txtchat3 = new TextBox();
            btndisconnect = new Button();
            btnclear = new Button();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            btnpicture = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // rtbMessages
            // 
            rtbMessages.BackColor = SystemColors.GradientInactiveCaption;
            rtbMessages.Location = new Point(11, 2);
            rtbMessages.Margin = new Padding(4);
            rtbMessages.Name = "rtbMessages";
            rtbMessages.ReadOnly = true;
            rtbMessages.Size = new Size(582, 460);
            rtbMessages.TabIndex = 2;
            rtbMessages.Text = "";
            // 
            // txtMessage
            // 
            txtMessage.Location = new Point(5, 515);
            txtMessage.Margin = new Padding(4);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(720, 58);
            txtMessage.TabIndex = 3;
            // 
            // btnSend
            // 
            btnSend.Font = new Font("Microsoft JhengHei UI", 16F);
            btnSend.Location = new Point(733, 516);
            btnSend.Margin = new Padding(4);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(93, 58);
            btnSend.TabIndex = 4;
            btnSend.Text = "發送";
            // 
            // txtchat1
            // 
            txtchat1.Font = new Font("Microsoft JhengHei UI", 16F);
            txtchat1.Location = new Point(29, 470);
            txtchat1.Margin = new Padding(4);
            txtchat1.Multiline = true;
            txtchat1.Name = "txtchat1";
            txtchat1.Size = new Size(139, 40);
            txtchat1.TabIndex = 5;
            txtchat1.Text = "您好!";
            // 
            // btnchat1
            // 
            btnchat1.Font = new Font("Microsoft JhengHei UI", 16F);
            btnchat1.Location = new Point(533, 471);
            btnchat1.Margin = new Padding(4, 2, 4, 2);
            btnchat1.Name = "btnchat1";
            btnchat1.Size = new Size(96, 42);
            btnchat1.TabIndex = 6;
            btnchat1.Text = "傳送";
            btnchat1.UseVisualStyleBackColor = true;
            // 
            // btnchat2
            // 
            btnchat2.Font = new Font("Microsoft JhengHei UI", 16F);
            btnchat2.Location = new Point(176, 470);
            btnchat2.Margin = new Padding(4, 2, 4, 2);
            btnchat2.Name = "btnchat2";
            btnchat2.Size = new Size(96, 42);
            btnchat2.TabIndex = 8;
            btnchat2.Text = "傳送";
            btnchat2.UseVisualStyleBackColor = true;
            // 
            // txtchat2
            // 
            txtchat2.Font = new Font("Microsoft JhengHei UI", 16F);
            txtchat2.Location = new Point(386, 473);
            txtchat2.Margin = new Padding(4);
            txtchat2.Multiline = true;
            txtchat2.Name = "txtchat2";
            txtchat2.Size = new Size(139, 40);
            txtchat2.TabIndex = 7;
            txtchat2.Text = "再見";
            // 
            // btnchat3
            // 
            btnchat3.Font = new Font("Microsoft JhengHei UI", 16F);
            btnchat3.Location = new Point(856, 472);
            btnchat3.Margin = new Padding(4, 2, 4, 2);
            btnchat3.Name = "btnchat3";
            btnchat3.Size = new Size(96, 38);
            btnchat3.TabIndex = 10;
            btnchat3.Text = "傳送";
            btnchat3.UseVisualStyleBackColor = true;
            // 
            // txtchat3
            // 
            txtchat3.Font = new Font("Microsoft JhengHei UI", 16F);
            txtchat3.Location = new Point(707, 471);
            txtchat3.Margin = new Padding(4);
            txtchat3.Multiline = true;
            txtchat3.Name = "txtchat3";
            txtchat3.Size = new Size(139, 39);
            txtchat3.TabIndex = 9;
            txtchat3.Text = "(表情)";
            // 
            // btndisconnect
            // 
            btndisconnect.Font = new Font("Microsoft JhengHei UI", 16F);
            btndisconnect.Location = new Point(386, 579);
            btndisconnect.Margin = new Padding(4);
            btndisconnect.Name = "btndisconnect";
            btndisconnect.Size = new Size(573, 44);
            btndisconnect.TabIndex = 11;
            btndisconnect.Text = "斷開連線";
            // 
            // btnclear
            // 
            btnclear.Font = new Font("Microsoft JhengHei UI", 11F);
            btnclear.Location = new Point(5, 577);
            btnclear.Margin = new Padding(4);
            btnclear.Name = "btnclear";
            btnclear.Size = new Size(349, 46);
            btnclear.TabIndex = 4;
            btnclear.Text = "清除聊天室";
            btnclear.Click += btnclear_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.ButtonShadow;
            pictureBox1.Location = new Point(602, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(369, 259);
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft JhengHei UI", 16F);
            label1.Location = new Point(713, 264);
            label1.Name = "label1";
            label1.Size = new Size(166, 28);
            label1.TabIndex = 13;
            label1.Text = "對方傳送的圖片";
            // 
            // btnpicture
            // 
            btnpicture.Font = new Font("Microsoft JhengHei UI", 16F);
            btnpicture.Location = new Point(841, 516);
            btnpicture.Name = "btnpicture";
            btnpicture.Size = new Size(118, 57);
            btnpicture.TabIndex = 14;
            btnpicture.Text = "傳送圖片";
            btnpicture.UseVisualStyleBackColor = true;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(971, 631);
            Controls.Add(btnpicture);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(btndisconnect);
            Controls.Add(btnchat3);
            Controls.Add(txtchat3);
            Controls.Add(btnchat2);
            Controls.Add(txtchat2);
            Controls.Add(btnchat1);
            Controls.Add(txtchat1);
            Controls.Add(rtbMessages);
            Controls.Add(txtMessage);
            Controls.Add(btnclear);
            Controls.Add(btnSend);
            Margin = new Padding(4);
            Name = "ChatForm";
            Text = "P2P 聊天(客戶端)";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();

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