using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace P2PServer
{
    partial class ServerPictureForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnPCpicture = new System.Windows.Forms.Button();
            this.btnlinkpicture = new System.Windows.Forms.Button();
            this.btnsend = new System.Windows.Forms.Button();
            this.btncancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pictureBox1.Location = new System.Drawing.Point(1, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(982, 510);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnPCpicture
            // 
            this.btnPCpicture.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F);
            this.btnPCpicture.Location = new System.Drawing.Point(12, 534);
            this.btnPCpicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPCpicture.Name = "btnPCpicture";
            this.btnPCpicture.Size = new System.Drawing.Size(169, 55);
            this.btnPCpicture.TabIndex = 1;
            this.btnPCpicture.Text = "選擇本機圖片";
            this.btnPCpicture.UseVisualStyleBackColor = true;
            // 
            // btnlinkpicture
            // 
            this.btnlinkpicture.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F);
            this.btnlinkpicture.Location = new System.Drawing.Point(187, 534);
            this.btnlinkpicture.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnlinkpicture.Name = "btnlinkpicture";
            this.btnlinkpicture.Size = new System.Drawing.Size(169, 55);
            this.btnlinkpicture.TabIndex = 1;
            this.btnlinkpicture.Text = "上傳圖片連結";
            this.btnlinkpicture.UseVisualStyleBackColor = true;
            // 
            // btnsend
            // 
            this.btnsend.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F);
            this.btnsend.Location = new System.Drawing.Point(639, 534);
            this.btnsend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnsend.Name = "btnsend";
            this.btnsend.Size = new System.Drawing.Size(169, 55);
            this.btnsend.TabIndex = 1;
            this.btnsend.Text = "傳送";
            this.btnsend.UseVisualStyleBackColor = true;
            // 
            // btncancel
            // 
            this.btncancel.Font = new System.Drawing.Font("Microsoft JhengHei UI", 16F);
            this.btncancel.Location = new System.Drawing.Point(814, 534);
            this.btncancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(169, 55);
            this.btncancel.TabIndex = 1;
            this.btncancel.Text = "取消";
            this.btncancel.UseVisualStyleBackColor = true;
            // 
            // Pictureform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 611);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.btnsend);
            this.Controls.Add(this.btnlinkpicture);
            this.Controls.Add(this.btnPCpicture);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Pictureform";
            this.Text = "選擇傳送圖片";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureBox1;
        private Button btnPCpicture;
        private Button btnlinkpicture;
        private Button btnsend;
        private Button btncancel;
    }
}