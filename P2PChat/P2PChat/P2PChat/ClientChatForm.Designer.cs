using System.Drawing;
using System.Windows.Forms;

namespace P2PChat
{
    partial class ClientChatForm
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
            lblword = new Label();
            btnreadpic = new Button();
            btnemoji = new Button();
            panel1 = new Panel();
            pictureBox2 = new PictureBox();
            label2 = new Label();
            flpMessages = new FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // txtMessage
            // 
            txtMessage.BackColor = SystemColors.Window;
            txtMessage.Font = new Font("新細明體", 14F);
            txtMessage.Location = new Point(4, 44);
            txtMessage.Margin = new Padding(4);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(724, 47);
            txtMessage.TabIndex = 3;
            // 
            // btnSend
            // 
            btnSend.BackColor = SystemColors.Window;
            btnSend.Font = new Font("Microsoft JhengHei UI", 16F);
            btnSend.Location = new Point(844, 0);
            btnSend.Margin = new Padding(4);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(108, 95);
            btnSend.TabIndex = 4;
            btnSend.Text = "發送";
            btnSend.UseVisualStyleBackColor = false;
            // 
            // txtchat1
            // 
            txtchat1.Font = new Font("Microsoft JhengHei UI", 16F);
            txtchat1.Location = new Point(4, 7);
            txtchat1.Margin = new Padding(4);
            txtchat1.Multiline = true;
            txtchat1.Name = "txtchat1";
            txtchat1.Size = new Size(155, 33);
            txtchat1.TabIndex = 5;
            txtchat1.Text = "您好!";
            // 
            // btnchat1
            // 
            btnchat1.Font = new Font("Microsoft JhengHei UI", 16F);
            btnchat1.Location = new Point(167, 7);
            btnchat1.Margin = new Padding(4, 2, 4, 2);
            btnchat1.Name = "btnchat1";
            btnchat1.Size = new Size(100, 33);
            btnchat1.TabIndex = 6;
            btnchat1.Text = "傳送";
            btnchat1.UseVisualStyleBackColor = true;
            // 
            // btnchat2
            // 
            btnchat2.Font = new Font("Microsoft JhengHei UI", 16F);
            btnchat2.Location = new Point(457, 7);
            btnchat2.Margin = new Padding(4, 2, 4, 2);
            btnchat2.Name = "btnchat2";
            btnchat2.Size = new Size(100, 33);
            btnchat2.TabIndex = 8;
            btnchat2.Text = "傳送";
            btnchat2.UseVisualStyleBackColor = true;
            // 
            // txtchat2
            // 
            txtchat2.Font = new Font("Microsoft JhengHei UI", 16F);
            txtchat2.Location = new Point(294, 7);
            txtchat2.Margin = new Padding(4);
            txtchat2.Multiline = true;
            txtchat2.Name = "txtchat2";
            txtchat2.Size = new Size(155, 33);
            txtchat2.TabIndex = 7;
            txtchat2.Text = "再見";
            // 
            // btnchat3
            // 
            btnchat3.Font = new Font("Microsoft JhengHei UI", 16F);
            btnchat3.Location = new Point(728, 5);
            btnchat3.Margin = new Padding(4, 2, 4, 2);
            btnchat3.Name = "btnchat3";
            btnchat3.Size = new Size(100, 33);
            btnchat3.TabIndex = 10;
            btnchat3.Text = "傳送";
            btnchat3.UseVisualStyleBackColor = true;
            // 
            // txtchat3
            // 
            txtchat3.Font = new Font("Microsoft JhengHei UI", 16F);
            txtchat3.Location = new Point(565, 7);
            txtchat3.Margin = new Padding(4);
            txtchat3.Multiline = true;
            txtchat3.Name = "txtchat3";
            txtchat3.Size = new Size(155, 33);
            txtchat3.TabIndex = 9;
            txtchat3.Text = "沒問題";
            // 
            // btndisconnect
            // 
            btndisconnect.BackColor = SystemColors.Window;
            btndisconnect.Font = new Font("Microsoft JhengHei UI", 16F);
            btndisconnect.Location = new Point(792, 481);
            btndisconnect.Margin = new Padding(4);
            btndisconnect.Name = "btndisconnect";
            btndisconnect.Size = new Size(149, 35);
            btndisconnect.TabIndex = 11;
            btndisconnect.Text = "斷開連線";
            btndisconnect.UseVisualStyleBackColor = false;
            // 
            // btnclear
            // 
            btnclear.BackColor = SystemColors.Window;
            btnclear.Font = new Font("Microsoft JhengHei UI", 16F);
            btnclear.Location = new Point(580, 481);
            btnclear.Margin = new Padding(4);
            btnclear.Name = "btnclear";
            btnclear.Size = new Size(149, 38);
            btnclear.TabIndex = 4;
            btnclear.Text = "清除聊天室";
            btnclear.UseVisualStyleBackColor = false;
            btnclear.Click += btnclear_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.ButtonShadow;
            pictureBox1.Location = new Point(574, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(376, 203);
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft JhengHei UI", 16F);
            label1.Location = new Point(682, 2);
            label1.Name = "label1";
            label1.Size = new Size(166, 28);
            label1.TabIndex = 13;
            label1.Text = "對方傳送的圖片";
            // 
            // btnpicture
            // 
            btnpicture.Font = new Font("Microsoft JhengHei UI", 16F);
            btnpicture.Location = new Point(792, 436);
            btnpicture.Name = "btnpicture";
            btnpicture.Size = new Size(148, 38);
            btnpicture.TabIndex = 14;
            btnpicture.Text = "傳送圖片";
            btnpicture.UseVisualStyleBackColor = true;
            // 
            // lblword
            // 
            lblword.AutoSize = true;
            lblword.Font = new Font("新細明體", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 136);
            lblword.Location = new Point(4, 502);
            lblword.Name = "lblword";
            lblword.Size = new Size(130, 21);
            lblword.TabIndex = 15;
            lblword.Text = "已輸入字數:0";
            // 
            // btnreadpic
            // 
            btnreadpic.Font = new Font("Microsoft JhengHei UI", 16F);
            btnreadpic.Location = new Point(580, 436);
            btnreadpic.Name = "btnreadpic";
            btnreadpic.Size = new Size(148, 38);
            btnreadpic.TabIndex = 16;
            btnreadpic.Text = "查看圖片";
            btnreadpic.UseVisualStyleBackColor = true;
            // 
            // btnemoji
            // 
            btnemoji.BackColor = SystemColors.Menu;
            btnemoji.Font = new Font("Microsoft JhengHei UI", 16F);
            btnemoji.Location = new Point(728, 46);
            btnemoji.Margin = new Padding(4);
            btnemoji.Name = "btnemoji";
            btnemoji.Size = new Size(108, 46);
            btnemoji.TabIndex = 4;
            btnemoji.Text = "表情選單";
            btnemoji.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(txtchat1);
            panel1.Controls.Add(txtMessage);
            panel1.Controls.Add(btnSend);
            panel1.Controls.Add(btnemoji);
            panel1.Controls.Add(btnchat1);
            panel1.Controls.Add(txtchat2);
            panel1.Controls.Add(btnchat3);
            panel1.Controls.Add(btnchat2);
            panel1.Controls.Add(txtchat3);
            panel1.Location = new Point(0, 526);
            panel1.Name = "panel1";
            panel1.Size = new Size(952, 95);
            panel1.TabIndex = 17;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = SystemColors.ButtonShadow;
            pictureBox2.Location = new Point(574, 211);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(376, 219);
            pictureBox2.TabIndex = 12;
            pictureBox2.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft JhengHei UI", 16F);
            label2.Location = new Point(692, 405);
            label2.Name = "label2";
            label2.Size = new Size(144, 28);
            label2.TabIndex = 13;
            label2.Text = "我傳送的圖片";
            // 
            // flpMessages
            // 
            flpMessages.AutoScroll = true;
            flpMessages.BackColor = SystemColors.GradientInactiveCaption;
            flpMessages.FlowDirection = FlowDirection.TopDown;
            flpMessages.Font = new Font("新細明體", 12F, FontStyle.Regular, GraphicsUnit.Point, 136);
            flpMessages.Location = new Point(4, 2);
            flpMessages.Name = "flpMessages";
            flpMessages.Size = new Size(570, 497);
            flpMessages.TabIndex = 18;
            flpMessages.WrapContents = false;
            // 
            // ClientChatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = SystemColors.Control;
            ClientSize = new Size(952, 621);
            Controls.Add(flpMessages);
            Controls.Add(btnreadpic);
            Controls.Add(lblword);
            Controls.Add(btnpicture);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(btndisconnect);
            Controls.Add(btnclear);
            Controls.Add(panel1);
            Margin = new Padding(4);
            Name = "ClientChatForm";
            Text = "P2P聊天室(客戶端)";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

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
        private Label lblword;
        private Button btnreadpic;
        private Button btnemoji;
        private Panel panel1;
        private PictureBox pictureBox2;
        private Label label2;
        private FlowLayoutPanel flpMessages;
    }
}