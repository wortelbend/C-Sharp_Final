using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace P2PServer
{
    // 圖片預覽視窗，提供圖片顯示和儲存功能
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

        // 設定要顯示的圖片，並自動調整顯示模式
        public void SetImage(Image image)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
            pictureBox1.Image = image;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        // 儲存圖片功能，支援多種圖片格式
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