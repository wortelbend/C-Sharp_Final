namespace P2PChat
{
    partial class ClientConnectForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblClientIP = new Label();
            txtClientIP = new TextBox();
            lblClientPORT = new Label();
            txtClientPORT = new TextBox();
            btnEnableTCP = new Button();
            btnDisableTCP = new Button();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            btnavater = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // lblClientIP
            // 
            lblClientIP.AutoSize = true;
            lblClientIP.Font = new Font("Microsoft JhengHei UI", 16F);
            lblClientIP.Location = new Point(84, 194);
            lblClientIP.Name = "lblClientIP";
            lblClientIP.Size = new Size(75, 28);
            lblClientIP.TabIndex = 0;
            lblClientIP.Text = "IP位址";
            lblClientIP.Click += btnEnableTCP_Click;
            // 
            // txtClientIP
            // 
            txtClientIP.Font = new Font("Microsoft JhengHei UI", 16F);
            txtClientIP.Location = new Point(180, 191);
            txtClientIP.Name = "txtClientIP";
            txtClientIP.Size = new Size(207, 35);
            txtClientIP.TabIndex = 1;
            // 
            // lblClientPORT
            // 
            lblClientPORT.AutoSize = true;
            lblClientPORT.Font = new Font("Microsoft JhengHei UI", 16F);
            lblClientPORT.Location = new Point(410, 198);
            lblClientPORT.Name = "lblClientPORT";
            lblClientPORT.Size = new Size(56, 28);
            lblClientPORT.TabIndex = 2;
            lblClientPORT.Text = "閘道";
            // 
            // txtClientPORT
            // 
            txtClientPORT.Font = new Font("Microsoft JhengHei UI", 16F);
            txtClientPORT.Location = new Point(472, 194);
            txtClientPORT.Name = "txtClientPORT";
            txtClientPORT.Size = new Size(198, 35);
            txtClientPORT.TabIndex = 3;
            // 
            // btnEnableTCP
            // 
            btnEnableTCP.Font = new Font("Microsoft JhengHei UI", 16F);
            btnEnableTCP.Location = new Point(180, 299);
            btnEnableTCP.Name = "btnEnableTCP";
            btnEnableTCP.Size = new Size(183, 93);
            btnEnableTCP.TabIndex = 4;
            btnEnableTCP.Text = "進行連線";
            btnEnableTCP.UseVisualStyleBackColor = true;
            // 
            // btnDisableTCP
            // 
            btnDisableTCP.Font = new Font("Microsoft JhengHei UI", 16F);
            btnDisableTCP.Location = new Point(472, 299);
            btnDisableTCP.Name = "btnDisableTCP";
            btnDisableTCP.Size = new Size(198, 93);
            btnDisableTCP.TabIndex = 5;
            btnDisableTCP.Text = "關閉程式";
            btnDisableTCP.UseVisualStyleBackColor = true;
            btnDisableTCP.Click += btnDisableTCP_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft JhengHei UI", 16F);
            label1.Location = new Point(122, 103);
            label1.Name = "label1";
            label1.Size = new Size(144, 28);
            label1.TabIndex = 0;
            label1.Text = "請先選擇頭像";
            label1.Click += btnEnableTCP_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = SystemColors.ControlDark;
            pictureBox2.Location = new Point(428, 23);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(200, 154);
            pictureBox2.TabIndex = 6;
            pictureBox2.TabStop = false;
            // 
            // btnavater
            // 
            btnavater.Font = new Font("Microsoft JhengHei UI", 16F);
            btnavater.Location = new Point(281, 95);
            btnavater.Name = "btnavater";
            btnavater.Size = new Size(144, 36);
            btnavater.TabIndex = 4;
            btnavater.Text = "選擇頭像";
            btnavater.UseVisualStyleBackColor = true;
            // 
            // ClientConnectForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pictureBox2);
            Controls.Add(btnDisableTCP);
            Controls.Add(btnavater);
            Controls.Add(btnEnableTCP);
            Controls.Add(txtClientPORT);
            Controls.Add(lblClientPORT);
            Controls.Add(txtClientIP);
            Controls.Add(label1);
            Controls.Add(lblClientIP);
            Name = "ClientConnectForm";
            Text = "客戶端登入畫面";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblClientIP;
        private TextBox txtClientIP;
        private Label lblClientPORT;
        private TextBox txtClientPORT;
        private Button btnEnableTCP;
        private Button btnDisableTCP;
        private Label label1;
        private PictureBox pictureBox2;
        private Button btnavater;
    }
}
