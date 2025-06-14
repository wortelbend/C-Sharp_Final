namespace P2PServer
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.txtServerPORT = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnEnableServerTCP = new System.Windows.Forms.Button();
            this.btndisbleServerTCP = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnavater = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 16F);
            this.label1.Location = new System.Drawing.Point(58, 213);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP位址";
            // 
            // txtServerIP
            // 
            this.txtServerIP.Font = new System.Drawing.Font("新細明體", 16F);
            this.txtServerIP.Location = new System.Drawing.Point(151, 210);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(204, 33);
            this.txtServerIP.TabIndex = 1;
            // 
            // txtServerPORT
            // 
            this.txtServerPORT.Font = new System.Drawing.Font("新細明體", 16F);
            this.txtServerPORT.Location = new System.Drawing.Point(485, 213);
            this.txtServerPORT.Name = "txtServerPORT";
            this.txtServerPORT.Size = new System.Drawing.Size(208, 33);
            this.txtServerPORT.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 16F);
            this.label2.Location = new System.Drawing.Point(391, 216);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "閘道";
            // 
            // btnEnableServerTCP
            // 
            this.btnEnableServerTCP.Font = new System.Drawing.Font("新細明體", 16F);
            this.btnEnableServerTCP.Location = new System.Drawing.Point(158, 302);
            this.btnEnableServerTCP.Name = "btnEnableServerTCP";
            this.btnEnableServerTCP.Size = new System.Drawing.Size(197, 82);
            this.btnEnableServerTCP.TabIndex = 4;
            this.btnEnableServerTCP.Text = "進行監聽";
            this.btnEnableServerTCP.UseVisualStyleBackColor = true;
            // 
            // btndisbleServerTCP
            // 
            this.btndisbleServerTCP.Font = new System.Drawing.Font("新細明體", 16F);
            this.btndisbleServerTCP.Location = new System.Drawing.Point(485, 302);
            this.btndisbleServerTCP.Name = "btndisbleServerTCP";
            this.btndisbleServerTCP.Size = new System.Drawing.Size(198, 82);
            this.btndisbleServerTCP.TabIndex = 5;
            this.btndisbleServerTCP.Text = "關閉程式";
            this.btndisbleServerTCP.UseVisualStyleBackColor = true;
            this.btndisbleServerTCP.Click += new System.EventHandler(this.btndisbleServerTCP_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 16F);
            this.label3.Location = new System.Drawing.Point(122, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 22);
            this.label3.TabIndex = 0;
            this.label3.Text = "請先選擇頭像";
            // 
            // btnavater
            // 
            this.btnavater.Font = new System.Drawing.Font("新細明體", 16F);
            this.btnavater.Location = new System.Drawing.Point(281, 95);
            this.btnavater.Name = "btnavater";
            this.btnavater.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnavater.Size = new System.Drawing.Size(114, 36);
            this.btnavater.TabIndex = 4;
            this.btnavater.Text = "選擇頭像";
            this.btnavater.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pictureBox2.Location = new System.Drawing.Point(428, 23);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(200, 154);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btndisbleServerTCP);
            this.Controls.Add(this.btnavater);
            this.Controls.Add(this.btnEnableServerTCP);
            this.Controls.Add(this.txtServerPORT);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtServerIP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "伺服器登入畫面";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.TextBox txtServerPORT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnEnableServerTCP;
        private System.Windows.Forms.Button btndisbleServerTCP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnavater;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

