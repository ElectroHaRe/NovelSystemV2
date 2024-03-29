﻿namespace WFControlLibrary
{
    using System;
    using Library;
    using System.Drawing;
    using System.Windows.Forms;

    partial class FieldElement
    {
        #region Код, автоматически созданный конструктором компонентов

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.textBox = new System.Windows.Forms.TextBox();
            this.noImageLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(250, 80);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageFieldMouseDown);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageFieldMouseUp);
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(-1, 80);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(251, 80);
            this.textBox.TabIndex = 1;
            // 
            // noImageLabel
            // 
            this.noImageLabel.AutoEllipsis = true;
            this.noImageLabel.Location = new System.Drawing.Point(98, 33);
            this.noImageLabel.Name = "noImageLabel";
            this.noImageLabel.Size = new System.Drawing.Size(53, 13);
            this.noImageLabel.TabIndex = 2;
            this.noImageLabel.Text = "No Image";
            this.noImageLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LabelMouseDown);
            this.noImageLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LabelMouseUp);
            // 
            // FieldElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.noImageLabel);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.pictureBox);
            this.DoubleBuffered = true;
            this.Name = "FieldElement";
            this.Size = new System.Drawing.Size(250, 160);
            this.SizeChanged += new System.EventHandler(this.OnSizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.IContainer components = null;

        private Label noImageLabel;
        private PictureBox pictureBox;
        private TextBox textBox;

        private const int maxWidth = 53;
        public const int maxHeight = 13;

        public new event EventHandler TextChanged
        {
            add { textBox.TextChanged += value; }
            remove { textBox.TextChanged -= value; }
        }
        /// <summary>
        /// sender - PictureBox, 1'Image - lastImage, 2'Image - currentImage
        /// </summary>
        public event Action<PictureBox, Image, Image> ImageChanged;
        public event MouseEventHandler ImageFieldMouseClick
        {
            add { pictureBox.MouseClick += value; }
            remove { pictureBox.MouseClick -= value; }
        }
        public new event MouseEventHandler MouseDown;
        public new event MouseEventHandler MouseUp;
        public new event EventHandler MouseHover
        {
            add { pictureBox.MouseHover += value; }
            remove { pictureBox.MouseHover -= value; }
        }
        public new event EventHandler MouseLeave
        {
            add { pictureBox.MouseLeave += value; }
            remove { pictureBox.MouseLeave -= value; }
        }

        public new string Text { get => textBox.Text; set => (this as IScene).Text = value; }
        public Image Image { get => pictureBox.Image; set => (this as IScene).Image = value; }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ImageFieldMouseDown(object sender, MouseEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }
        private void ImageFieldMouseUp(object sender, MouseEventArgs e)
        {
            MouseUp?.Invoke(this, e);
        }
        private void LabelMouseDown(object sender, MouseEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }
        private void LabelMouseUp(object sender, MouseEventArgs e)
        {
            MouseUp?.Invoke(this, e);
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            pictureBox.Width = Width; pictureBox.Height = Height / 2;
            textBox.Width = Width + 1; textBox.Height = Height - pictureBox.Height;

            pictureBox.Left = 0; pictureBox.Top = 0;
            textBox.Left = -1; textBox.Top = pictureBox.Height + pictureBox.Top;

            noImageLabel.Width = Width < maxWidth ? Width : maxWidth;

            if (Image == null)
            {
                if (Height / 2 < maxHeight)
                    noImageLabel.Enabled = noImageLabel.Visible = false;
                else if (!noImageLabel.Enabled && !noImageLabel.Visible)
                    noImageLabel.Enabled = noImageLabel.Visible = true;
            }

            noImageLabel.Left = Width / 2 - noImageLabel.Width / 2; noImageLabel.Top = Height / 4 - noImageLabel.Height / 2;
        }
    }
}
