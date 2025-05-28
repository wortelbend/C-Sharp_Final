﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;

namespace P2PChat
{
    public partial class ClientViewPictureForm : Form
    {
        // 顯示、縮放及平移相關狀態變數
        private Image originalImage;
        private float currentZoomFactor = 1.0f;
        private PointF imageOffset = PointF.Empty;
        private bool isPanning = false;
        private Point lastPanPoint = Point.Empty;
        private const float ZOOM_STEP = 0.15f;

        // 表單建構函式
        public ClientViewPictureForm()
        {
            InitializeComponent();
            SetupButtonEventHandlers();

            this.pictureBox1.MouseWheel += PictureBox1_MouseWheel;
            this.pictureBox1.Paint += PictureBox1_Paint;
            this.pictureBox1.MouseDown += PictureBox1_MouseDown;
            this.pictureBox1.MouseMove += PictureBox1_MouseMove;
            this.pictureBox1.MouseUp += PictureBox1_MouseUp;

            this.pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            this.DoubleBuffered = true;
        }

        // UI按鈕的點擊事件處理器
        private void SetupButtonEventHandlers()
        {
            btnsave.Click += btnsave_Click;
            btnback.Click += btnback_Click;
        }

        // 顯示新的圖片，重設縮放和平移狀態
        public void SetImage(Image image)
        {
            this.originalImage = image;
            if (this.originalImage != null)
            {
                this.currentZoomFactor = 1.0f;
                CenterImageAndCheckBoundaries();
                this.pictureBox1.Invalidate();
            }
            else
            {
                this.imageOffset = PointF.Empty;
                this.pictureBox1.Invalidate();
            }
        }

        // 計算初始圖片偏移量以使其居中，並進行邊界檢查
        private void CenterImageAndCheckBoundaries()
        {
            if (originalImage == null) return;
            float scaledWidth = originalImage.Width * currentZoomFactor;
            float scaledHeight = originalImage.Height * currentZoomFactor;
            imageOffset.X = (pictureBox1.ClientSize.Width - scaledWidth) / 2f;
            imageOffset.Y = (pictureBox1.ClientSize.Height - scaledHeight) / 2f;
            BoundaryCheckImageOffset();
        }

        // PictureBox的繪圖事件，將圖片依目前縮放及偏移量繪製出來
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(this.pictureBox1.BackColor);

            if (this.originalImage == null)
            {
                TextRenderer.DrawText(e.Graphics, "沒有圖片", this.Font, this.pictureBox1.ClientRectangle, SystemColors.GrayText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                return;
            }

            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            float scaledWidth = this.originalImage.Width * this.currentZoomFactor;
            float scaledHeight = this.originalImage.Height * this.currentZoomFactor;

            RectangleF destinationRect = new RectangleF(this.imageOffset.X, this.imageOffset.Y, scaledWidth, scaledHeight);
            RectangleF sourceRect = new RectangleF(0, 0, this.originalImage.Width, this.originalImage.Height);

            e.Graphics.DrawImage(this.originalImage, destinationRect, sourceRect, GraphicsUnit.Pixel);
        }

        // 滑鼠滾輪事件，以游標為中心的圖片縮放
        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (this.originalImage == null) return;

            PointF mousePosInPictureBox = pictureBox1.PointToClient(Cursor.Position);
            float oldZoomFactor = this.currentZoomFactor;

            if (e.Delta > 0) this.currentZoomFactor += ZOOM_STEP;
            else this.currentZoomFactor -= ZOOM_STEP;
            this.currentZoomFactor = Math.Max(0.1f, Math.Min(this.currentZoomFactor, 10.0f));

            if (Math.Abs(oldZoomFactor - this.currentZoomFactor) < 0.001f) return;

            float imageX_at_mouse = (mousePosInPictureBox.X - this.imageOffset.X) / oldZoomFactor;
            float imageY_at_mouse = (mousePosInPictureBox.Y - this.imageOffset.Y) / oldZoomFactor;
            this.imageOffset.X = mousePosInPictureBox.X - (imageX_at_mouse * this.currentZoomFactor);
            this.imageOffset.Y = mousePosInPictureBox.Y - (imageY_at_mouse * this.currentZoomFactor);

            BoundaryCheckImageOffset();
            this.pictureBox1.Invalidate();
        }

        // 滑鼠按下事件
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && this.originalImage != null)
            {
                isPanning = true;
                lastPanPoint = e.Location;
                this.pictureBox1.Cursor = Cursors.SizeAll;
            }
        }

        // 滑鼠移動事件
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPanning && this.originalImage != null)
            {
                float deltaX = e.Location.X - lastPanPoint.X;
                float deltaY = e.Location.Y - lastPanPoint.Y;

                this.imageOffset.X += deltaX;
                this.imageOffset.Y += deltaY;

                lastPanPoint = e.Location;
                BoundaryCheckImageOffset();
                this.pictureBox1.Invalidate();
            }
        }

        // 滑鼠釋放事件
        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPanning = false;
                this.pictureBox1.Cursor = Cursors.Default;
            }
        }

        // 檢查並調整圖片偏移量，確保圖片內容不移出可視區域
        private void BoundaryCheckImageOffset()
        {
            if (originalImage == null) return;

            float scaledWidth = originalImage.Width * currentZoomFactor;
            float scaledHeight = originalImage.Height * currentZoomFactor;
            float pbClientWidth = pictureBox1.ClientSize.Width;
            float pbClientHeight = pictureBox1.ClientSize.Height;

            if (scaledWidth < pbClientWidth)
                imageOffset.X = (pbClientWidth - scaledWidth) / 2f;
            else
                imageOffset.X = Math.Min(0, Math.Max(pbClientWidth - scaledWidth, imageOffset.X));

            if (scaledHeight < pbClientHeight)
                imageOffset.Y = (pbClientHeight - scaledHeight) / 2f;
            else
                imageOffset.Y = Math.Min(0, Math.Max(pbClientHeight - scaledHeight, imageOffset.Y));
        }

        // 儲存目前顯示的圖片
        private void btnsave_Click(object sender, EventArgs e)
        {
            if (this.originalImage == null)
            {
                MessageBox.Show(this, "沒有圖片可儲存。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "JPEG 圖片|*.jpg|PNG 圖片|*.png|BMP 圖片|*.bmp|所有檔案|*.*",
                Title = "儲存圖片",
                FileName = "圖片_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                AddExtension = true,
                DefaultExt = "png"
            };

            if (saveDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    this.originalImage.Save(saveDialog.FileName);
                    MessageBox.Show(this, "圖片已成功儲存。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "儲存圖片時發生錯誤：" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 關閉目前視窗
        private void btnback_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}