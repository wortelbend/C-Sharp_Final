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
            SuspendLayout();
            // 
            // lblClientIP
            // 
            lblClientIP.AutoSize = true;
            lblClientIP.Font = new Font("Microsoft JhengHei UI", 16F);
            lblClientIP.Location = new Point(78, 149);
            lblClientIP.Name = "lblClientIP";
            lblClientIP.Size = new Size(75, 28);
            lblClientIP.TabIndex = 0;
            lblClientIP.Text = "IP位址";
            lblClientIP.Click += btnEnableTCP_Click;
            // 
            // txtClientIP
            // 
            txtClientIP.Font = new Font("Microsoft JhengHei UI", 16F);
            txtClientIP.Location = new Point(174, 146);
            txtClientIP.Name = "txtClientIP";
            txtClientIP.Size = new Size(207, 35);
            txtClientIP.TabIndex = 1;
            // 
            // lblClientPORT
            // 
            lblClientPORT.AutoSize = true;
            lblClientPORT.Font = new Font("Microsoft JhengHei UI", 16F);
            lblClientPORT.Location = new Point(404, 153);
            lblClientPORT.Name = "lblClientPORT";
            lblClientPORT.Size = new Size(56, 28);
            lblClientPORT.TabIndex = 2;
            lblClientPORT.Text = "閘道";
            // 
            // txtClientPORT
            // 
            txtClientPORT.Font = new Font("Microsoft JhengHei UI", 16F);
            txtClientPORT.Location = new Point(466, 149);
            txtClientPORT.Name = "txtClientPORT";
            txtClientPORT.Size = new Size(198, 35);
            txtClientPORT.TabIndex = 3;
            // 
            // btnEnableTCP
            // 
            btnEnableTCP.Font = new Font("Microsoft JhengHei UI", 16F);
            btnEnableTCP.Location = new Point(174, 254);
            btnEnableTCP.Name = "btnEnableTCP";
            btnEnableTCP.Size = new Size(183, 93);
            btnEnableTCP.TabIndex = 4;
            btnEnableTCP.Text = "進行連線";
            btnEnableTCP.UseVisualStyleBackColor = true;
            // 
            // btnDisableTCP
            // 
            btnDisableTCP.Font = new Font("Microsoft JhengHei UI", 16F);
            btnDisableTCP.Location = new Point(466, 254);
            btnDisableTCP.Name = "btnDisableTCP";
            btnDisableTCP.Size = new Size(198, 93);
            btnDisableTCP.TabIndex = 5;
            btnDisableTCP.Text = "關閉程式";
            btnDisableTCP.UseVisualStyleBackColor = true;
            btnDisableTCP.Click += btnDisableTCP_Click;
            // 
            // ClientConnectForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnDisableTCP);
            Controls.Add(btnEnableTCP);
            Controls.Add(txtClientPORT);
            Controls.Add(lblClientPORT);
            Controls.Add(txtClientIP);
            Controls.Add(lblClientIP);
            Name = "ClientConnectForm";
            Text = "客戶端登入畫面";
            Load += Form1_Load;
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
    }
}
