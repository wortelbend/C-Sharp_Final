using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace P2PChat
{
    public partial class ViewPictureForm : Form
    {
        public ViewPictureForm()
        {
            InitializeComponent();
            SetupEventHandlers();
        }

        private void SetupEventHandlers()
        {
            btnsave.Click += btnsave_Click;
            btnback.Click += btnback_Click;
        }

        public void SetImage(Image image)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }

            // 調整圖片大小
            if (image.Width > 1000 || image.Height > 650)
            {
                double ratio = Math.Min(1000.0 / image.Width, 650.0 / image.Height);
                int newWidth = (int)(image.Width * ratio);
                int newHeight = (int)(image.Height * ratio);

                Bitmap resizedImage = new Bitmap(newWidth, newHeight);
                using (Graphics g = Graphics.FromImage(resizedImage))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, 0, 0, newWidth, newHeight);
                }
                pictureBox1.Image = resizedImage;
            }
            else
            {
                pictureBox1.Image = image;
            }

            // 設定 PictureBox 的大小模式
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "JPEG 圖片|*.jpg|PNG 圖片|*.png|所有檔案|*.*";
                saveDialog.Title = "儲存圖片";
                saveDialog.FileName = "圖片_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox1.Image.Save(saveDialog.FileName);
                        MessageBox.Show("圖片已成功儲存", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("儲存圖片時發生錯誤：" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}