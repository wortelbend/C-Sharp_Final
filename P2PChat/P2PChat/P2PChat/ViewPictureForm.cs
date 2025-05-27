using System;
using System.Drawing;
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
        // 顯示圖片，並自動調整顯示模式
        public void SetImage(Image image)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
            pictureBox1.Image = image;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        // 儲存圖片按鈕的點擊事件
        private void btnsave_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                // 建立儲存檔案對話框
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "JPEG 圖片|*.jpg|PNG 圖片|*.png|所有檔案|*.*";
                saveDialog.Title = "儲存圖片";
                saveDialog.FileName = "圖片_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

                // 儲存對話框並處理使用者選擇
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 儲存圖片到本機選擇的位置
                        pictureBox1.Image.Save(saveDialog.FileName);
                        MessageBox.Show("圖片已成功儲存", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // 處理儲存過程中可能發生的錯誤
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