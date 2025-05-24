namespace P2PChat
{
    partial class formpicture
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            btnPCpicture = new Button();
            btnlinkpicture = new Button();
            btnsend = new Button();
            btncancel = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(776, 349);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // btnPCpicture
            // 
            btnPCpicture.Font = new Font("Microsoft JhengHei UI", 16F);
            btnPCpicture.Location = new Point(12, 370);
            btnPCpicture.Name = "btnPCpicture";
            btnPCpicture.Size = new Size(169, 55);
            btnPCpicture.TabIndex = 1;
            btnPCpicture.Text = "選擇本機圖片";
            btnPCpicture.UseVisualStyleBackColor = true;
            // 
            // btnlinkpicture
            // 
            btnlinkpicture.Font = new Font("Microsoft JhengHei UI", 16F);
            btnlinkpicture.Location = new Point(187, 370);
            btnlinkpicture.Name = "btnlinkpicture";
            btnlinkpicture.Size = new Size(169, 55);
            btnlinkpicture.TabIndex = 1;
            btnlinkpicture.Text = "上傳圖片連結";
            btnlinkpicture.UseVisualStyleBackColor = true;
            // 
            // btnsend
            // 
            btnsend.Font = new Font("Microsoft JhengHei UI", 16F);
            btnsend.Location = new Point(471, 370);
            btnsend.Name = "btnsend";
            btnsend.Size = new Size(135, 55);
            btnsend.TabIndex = 1;
            btnsend.Text = "傳送";
            btnsend.UseVisualStyleBackColor = true;
            // 
            // btncancel
            // 
            btncancel.Font = new Font("Microsoft JhengHei UI", 16F);
            btncancel.Location = new Point(671, 370);
            btncancel.Name = "btncancel";
            btncancel.Size = new Size(117, 55);
            btncancel.TabIndex = 1;
            btncancel.Text = "取消";
            btncancel.UseVisualStyleBackColor = true;
            // 
            // formpicture
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btncancel);
            Controls.Add(btnsend);
            Controls.Add(btnlinkpicture);
            Controls.Add(btnPCpicture);
            Controls.Add(pictureBox1);
            Name = "formpicture";
            Text = "選擇傳送圖片";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Button btnPCpicture;
        private Button btnlinkpicture;
        private Button btnsend;
        private Button btncancel;
    }
}