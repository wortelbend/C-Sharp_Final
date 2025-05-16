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
            lblClientIP.Location = new Point(47, 51);
            lblClientIP.Name = "lblClientIP";
            lblClientIP.Size = new Size(42, 15);
            lblClientIP.TabIndex = 0;
            lblClientIP.Text = "label1";
            lblClientIP.Click += label1_Click;
            // 
            // txtClientIP
            // 
            txtClientIP.Location = new Point(95, 48);
            txtClientIP.Name = "txtClientIP";
            txtClientIP.Size = new Size(100, 23);
            txtClientIP.TabIndex = 1;
            // 
            // lblClientPORT
            // 
            lblClientPORT.AutoSize = true;
            lblClientPORT.Location = new Point(295, 49);
            lblClientPORT.Name = "lblClientPORT";
            lblClientPORT.Size = new Size(42, 15);
            lblClientPORT.TabIndex = 2;
            lblClientPORT.Text = "label2";
            // 
            // txtClientPORT
            // 
            txtClientPORT.Location = new Point(343, 48);
            txtClientPORT.Name = "txtClientPORT";
            txtClientPORT.Size = new Size(100, 23);
            txtClientPORT.TabIndex = 3;
            // 
            // btnEnableTCP
            // 
            btnEnableTCP.Location = new Point(95, 86);
            btnEnableTCP.Name = "btnEnableTCP";
            btnEnableTCP.Size = new Size(75, 23);
            btnEnableTCP.TabIndex = 4;
            btnEnableTCP.Text = "button1";
            btnEnableTCP.UseVisualStyleBackColor = true;
            // 
            // btnDisableTCP
            // 
            btnDisableTCP.Location = new Point(343, 86);
            btnDisableTCP.Name = "btnDisableTCP";
            btnDisableTCP.Size = new Size(75, 23);
            btnDisableTCP.TabIndex = 5;
            btnDisableTCP.Text = "button2";
            btnDisableTCP.UseVisualStyleBackColor = true;
            // 
            // txtClientConnect
            // 
            txtClientConnect.AutoSize = true;
            txtClientConnect.Location = new Point(47, 132);
            txtClientConnect.Name = "txtClientConnect";
            txtClientConnect.Size = new Size(42, 15);
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
