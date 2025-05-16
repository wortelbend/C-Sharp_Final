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
            this.btnEnbleServerTCP = new System.Windows.Forms.Button();
            this.btndisbleServerTCP = new System.Windows.Forms.Button();
            this.txtServerConnect = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(117, 41);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(100, 22);
            this.txtServerIP.TabIndex = 1;
            // 
            // txtServerPORT
            // 
            this.txtServerPORT.Location = new System.Drawing.Point(390, 41);
            this.txtServerPORT.Name = "txtServerPORT";
            this.txtServerPORT.Size = new System.Drawing.Size(100, 22);
            this.txtServerPORT.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(351, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "label2";
            // 
            // btnEnbleServerTCP
            // 
            this.btnEnbleServerTCP.Location = new System.Drawing.Point(117, 85);
            this.btnEnbleServerTCP.Name = "btnEnbleServerTCP";
            this.btnEnbleServerTCP.Size = new System.Drawing.Size(75, 23);
            this.btnEnbleServerTCP.TabIndex = 4;
            this.btnEnbleServerTCP.Text = "button1";
            this.btnEnbleServerTCP.UseVisualStyleBackColor = true;
            // 
            // btndisbleServerTCP
            // 
            this.btndisbleServerTCP.Location = new System.Drawing.Point(390, 85);
            this.btndisbleServerTCP.Name = "btndisbleServerTCP";
            this.btndisbleServerTCP.Size = new System.Drawing.Size(75, 23);
            this.btndisbleServerTCP.TabIndex = 5;
            this.btndisbleServerTCP.Text = "button2";
            this.btndisbleServerTCP.UseVisualStyleBackColor = true;
            this.btndisbleServerTCP.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtServerConnect
            // 
            this.txtServerConnect.AutoSize = true;
            this.txtServerConnect.Location = new System.Drawing.Point(78, 149);
            this.txtServerConnect.Name = "txtServerConnect";
            this.txtServerConnect.Size = new System.Drawing.Size(33, 12);
            this.txtServerConnect.TabIndex = 6;
            this.txtServerConnect.Text = "label3";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtServerConnect);
            this.Controls.Add(this.btndisbleServerTCP);
            this.Controls.Add(this.btnEnbleServerTCP);
            this.Controls.Add(this.txtServerPORT);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtServerIP);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.TextBox txtServerPORT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnEnbleServerTCP;
        private System.Windows.Forms.Button btndisbleServerTCP;
        private System.Windows.Forms.Label txtServerConnect;
    }
}

