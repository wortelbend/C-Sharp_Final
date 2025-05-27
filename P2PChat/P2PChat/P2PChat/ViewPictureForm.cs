using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace P2PChat
{
    public partial class ViewPictureForm : Form
    {
        private Image _originalImage;     // 儲存原始圖片

        public ViewPictureForm()
        {
            InitializeComponent();
            SetupEventHandlers();
            // 移除或註釋掉滑鼠滾輪事件處理器，如果你只想使用按鈕
            // pictureBox1.MouseWheel += pictureBox1_MouseWheel; // 如果你仍然想保留滾輪功能，可以保留這一行
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
            _originalImage = image; // 將原始圖片儲存起來
            pictureBox1.Image = _originalImage; // 初始顯示原始圖片
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        // 儲存圖片按鈕的點擊事件
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