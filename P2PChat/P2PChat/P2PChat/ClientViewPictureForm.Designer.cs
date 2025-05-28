namespace P2PChat
{
    partial class ClientViewPictureForm
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
            btnsave = new Button();
            btnback = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = SystemColors.ControlDark;
            pictureBox1.Location = new Point(2, 1);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(981, 514);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // btnsave
            // 
            btnsave.Font = new Font("Microsoft JhengHei UI", 16F);
            btnsave.Location = new Point(556, 556);
            btnsave.Name = "btnsave";
            btnsave.Size = new Size(193, 43);
            btnsave.TabIndex = 1;
            btnsave.Text = "儲存圖片";
            btnsave.UseVisualStyleBackColor = true;
            // 
            // btnback
            // 
            btnback.Font = new Font("Microsoft JhengHei UI", 16F);
            btnback.Location = new Point(779, 556);
            btnback.Name = "btnback";
            btnback.Size = new Size(193, 43);
            btnback.TabIndex = 1;
            btnback.Text = "返回";
            btnback.UseVisualStyleBackColor = true;
            // 
            // ViewPictureForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 611);
            Controls.Add(btnback);
            Controls.Add(btnsave);
            Controls.Add(pictureBox1);
            Name = "ViewPictureForm";
            Text = "查看圖片";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Button btnsave;
        private Button btnback;
    }
}