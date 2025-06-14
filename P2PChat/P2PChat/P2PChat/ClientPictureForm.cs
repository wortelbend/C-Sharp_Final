using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace P2PChat
{
    public partial class formpicture : Form
    {
        private TcpClient _connectedClient;
        private Image selectedImage;

        // 這個屬性存放給外部使用的安全複本
        public Image ClonedImage { get; private set; }

        public formpicture(TcpClient connectedClient)
        {
            InitializeComponent();
            _connectedClient = connectedClient;
            SetupEventHandlers();
        }

        /// 用於設定視窗中所有按鈕
        private void SetupEventHandlers()
        {
            btnPCpicture.Click += btnPCpicture_Click;
            btnlinkpicture.Click += btnlinkpicture_Click;
            btnsend.Click += btnsend_Click;
            btncancel.Click += btncancel_Click;
        }

        /// 從本機電腦選擇圖片
        private void btnPCpicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "圖片檔案|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "選擇要傳送的圖片";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                        if (fileInfo.Length > 5 * 1024 * 1024)
                        {
                            MessageBox.Show("圖片大小不能超過 5MB", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        selectedImage = Image.FromFile(openFileDialog.FileName);
                        pictureBox1.Image = selectedImage;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("圖片讀取失敗: " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// 從連結選擇圖片
        private void btnlinkpicture_Click(object sender, EventArgs e)
        {
            using (Form inputForm = new Form())
            {
                inputForm.Text = "傳送圖片連結";
                inputForm.Width = 400;
                inputForm.Height = 150;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.MaximizeBox = false;
                inputForm.MinimizeBox = false;

                Label label = new Label();
                label.Text = "請輸入圖片網址：";
                label.SetBounds(10, 10, 380, 20);

                TextBox textBox = new TextBox();
                textBox.SetBounds(10, 30, 360, 20);
                textBox.Text = "https://";

                Button okButton = new Button();
                okButton.Text = "確定";
                okButton.DialogResult = DialogResult.OK;
                okButton.SetBounds(200, 60, 80, 25);

                Button cancelButton = new Button();
                cancelButton.Text = "取消";
                cancelButton.DialogResult = DialogResult.Cancel;
                cancelButton.SetBounds(290, 60, 80, 25);

                inputForm.Controls.AddRange(new Control[] { label, textBox, okButton, cancelButton });
                inputForm.AcceptButton = okButton;
                inputForm.CancelButton = cancelButton;

                if (inputForm.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(textBox.Text))
                {
                    try
                    {
                        using (WebClient client = new WebClient())
                        {
                            byte[] imageBytes = client.DownloadData(textBox.Text);
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                selectedImage = Image.FromStream(ms);
                                pictureBox1.Image = selectedImage;
                                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("圖片連結讀取失敗: " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// 點擊 傳送 按鈕
        private void btnsend_Click(object sender, EventArgs e)
        {
            if (selectedImage == null)
            {
                MessageBox.Show("請先選擇圖片", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    selectedImage.Save(ms, selectedImage.RawFormat);
                    byte[] imageBytes = ms.ToArray();
                    string base64Image = Convert.ToBase64String(imageBytes);
                    string imageMessage = $"<IMAGE>{base64Image}</IMAGE>";
                    byte[] data = Encoding.Unicode.GetBytes(imageMessage);

                    if (_connectedClient?.Connected == true)
                    {
                        _connectedClient.GetStream().Write(data, 0, data.Length);
                        this.ClonedImage = (Image)this.selectedImage.Clone();
                        MessageBox.Show("圖片已傳送", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("未連接到對方，無法傳送圖片", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("圖片傳送失敗: " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// 釋放已選擇圖片的資源，避免記憶體洩漏
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (selectedImage != null)
            {
                selectedImage.Dispose();
                selectedImage = null;
            }
        }
    }
}