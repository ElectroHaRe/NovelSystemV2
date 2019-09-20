namespace WFControlLibrary
{

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
            this.rootLabel = new System.Windows.Forms.Label();
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
            this.noImageLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LabelsMouseDown);
            this.noImageLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LabelsMouseUp);
            // 
            // rootLabel
            // 
            this.rootLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rootLabel.Location = new System.Drawing.Point(-1, -1);
            this.rootLabel.Name = "rootLabel";
            this.rootLabel.Size = new System.Drawing.Size(32, 17);
            this.rootLabel.TabIndex = 3;
            this.rootLabel.Text = "Root";
            this.rootLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LabelsMouseDown);
            this.rootLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LabelsMouseUp);
            // 
            // FieldElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.rootLabel);
            this.Controls.Add(this.noImageLabel);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.pictureBox);
            this.DoubleBuffered = true;
            this.Name = "FieldElement";
            this.Size = new System.Drawing.Size(250, 160);
            this.SizeChanged += new System.EventHandler(this.SizeChangedHandler);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label noImageLabel;
        private System.Windows.Forms.Label rootLabel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TextBox textBox;

        private const int max_label_width = 53;
        public const int max_label_height = 13;


        public new event System.EventHandler TextChanged
        {
            add { textBox.TextChanged += value; }
            remove { textBox.TextChanged -= value; }
        }
        /// <summary>
        /// sender - PictureBox
        /// </summary>
        public event System.EventHandler ImageChanged;
        public event System.Windows.Forms.MouseEventHandler ImageFieldMouseClick
        {
            add { pictureBox.MouseClick += value; }
            remove { pictureBox.MouseClick -= value; }
        }
        public new event System.Windows.Forms.MouseEventHandler MouseDown;
        public new event System.Windows.Forms.MouseEventHandler MouseUp;
        public new event System.EventHandler MouseHover
        {
            add { pictureBox.MouseHover += value; }
            remove { pictureBox.MouseHover -= value; }
        }
        public new event System.EventHandler MouseLeave
        {
            add { pictureBox.MouseLeave += value; }
            remove { pictureBox.MouseLeave -= value; }
        }
        public event System.EventHandler RealPositionChanged;
        public event System.EventHandler RealSizeChanged;

        public new string Text { get => textBox.Text; set => (this as Library.IScene).ChangeText(value); }
        public System.Drawing.Image Image { get => pictureBox.Image; set => (this as Library.IScene).ChangeImage(value); }

        public System.Drawing.Point RealPosition { get => (this as IFieldElement).Position; set => (this as IFieldElement).ChangePosition(value); }
        public System.Drawing.Size RealSize { get => (this as IFieldElement).Size; set => (this as IFieldElement).ChangeSize(value); }

        public bool rootMarker { get => rootLabel.Visible && rootLabel.Enabled; set => rootLabel.Visible = rootLabel.Enabled = value; }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ImageFieldMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }
        private void ImageFieldMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseUp?.Invoke(this, e);
        }
        private void LabelsMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseDown?.Invoke(this, e);
        }
        private void LabelsMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseUp?.Invoke(this, e);
        }

        private void SizeChangedHandler(object sender, System.EventArgs e)
        {
            pictureBox.Width = Width; pictureBox.Height = Height / 2;
            textBox.Width = Width + 1; textBox.Height = Height - pictureBox.Height;

            pictureBox.Left = 0; pictureBox.Top = 0;
            textBox.Left = -1; textBox.Top = pictureBox.Height + pictureBox.Top;

            rootLabel.Left = -1; rootLabel.Top = -1;

            noImageLabel.Width = Width < max_label_width ? Width : max_label_width;

            if (Image == null)
            {
                if (Height / 2 < max_label_height)
                    noImageLabel.Enabled = noImageLabel.Visible = false;
                else if (!noImageLabel.Enabled && !noImageLabel.Visible)
                    noImageLabel.Enabled = noImageLabel.Visible = true;
            }

            noImageLabel.Left = Width / 2 - noImageLabel.Width / 2; noImageLabel.Top = Height / 4 - noImageLabel.Height / 2;
        }
    }
}
