namespace P2PChat
{
    partial class Form1
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
            txtClientConnect = new Label();
            SuspendLayout();
            // 
            // lblClientIP
            // 
            lblClientIP.AutoSize = true;
            lblClientIP.Font = new Font("Microsoft JhengHei UI", 16F);
            lblClientIP.Location = new Point(12, 58);
            lblClientIP.Name = "lblClientIP";
            lblClientIP.Size = new Size(75, 28);
            lblClientIP.TabIndex = 0;
            lblClientIP.Text = "IP位址";
            lblClientIP.Click += btnEnableTCP_Click;
            // 
            // txtClientIP
            // 
            txtClientIP.Font = new Font("Microsoft JhengHei UI", 16F);
            txtClientIP.Location = new Point(105, 55);
            txtClientIP.Name = "txtClientIP";
            txtClientIP.Size = new Size(153, 35);
            txtClientIP.TabIndex = 1;
            // 
            // lblClientPORT
            // 
            lblClientPORT.AutoSize = true;
            lblClientPORT.Font = new Font("Microsoft JhengHei UI", 16F);
            lblClientPORT.Location = new Point(307, 62);
            lblClientPORT.Name = "lblClientPORT";
            lblClientPORT.Size = new Size(56, 28);
            lblClientPORT.TabIndex = 2;
            lblClientPORT.Text = "閘道";
            // 
            // txtClientPORT
            // 
            txtClientPORT.Font = new Font("Microsoft JhengHei UI", 16F);
            txtClientPORT.Location = new Point(369, 58);
            txtClientPORT.Name = "txtClientPORT";
            txtClientPORT.Size = new Size(134, 35);
            txtClientPORT.TabIndex = 3;
            // 
            // btnEnableTCP
            // 
            btnEnableTCP.Font = new Font("Microsoft JhengHei UI", 16F);
            btnEnableTCP.Location = new Point(95, 159);
            btnEnableTCP.Name = "btnEnableTCP";
            btnEnableTCP.Size = new Size(129, 54);
            btnEnableTCP.TabIndex = 4;
            btnEnableTCP.Text = "啟用IP";
            btnEnableTCP.UseVisualStyleBackColor = true;
            // 
            // btnDisableTCP
            // 
            btnDisableTCP.Font = new Font("Microsoft JhengHei UI", 16F);
            btnDisableTCP.Location = new Point(343, 159);
            btnDisableTCP.Name = "btnDisableTCP";
            btnDisableTCP.Size = new Size(144, 54);
            btnDisableTCP.TabIndex = 5;
            btnDisableTCP.Text = "關閉程式";
            btnDisableTCP.UseVisualStyleBackColor = true;
            btnDisableTCP.Click += btnDisableTCP_Click;
            // 
            // txtClientConnect
            // 
            txtClientConnect.AutoSize = true;
            txtClientConnect.Font = new Font("Microsoft JhengHei UI", 16F);
            txtClientConnect.Location = new Point(56, 238);
            txtClientConnect.Name = "txtClientConnect";
            txtClientConnect.Size = new Size(75, 28);
            txtClientConnect.TabIndex = 6;
            txtClientConnect.Text = "label3";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtClientConnect);
            Controls.Add(btnDisableTCP);
            Controls.Add(btnEnableTCP);
            Controls.Add(txtClientPORT);
            Controls.Add(lblClientPORT);
            Controls.Add(txtClientIP);
            Controls.Add(lblClientIP);
            Name = "Form1";
            Text = "Form1";
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
        private Label txtClientConnect;
    }
}
