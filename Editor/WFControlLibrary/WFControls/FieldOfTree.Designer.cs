namespace WFControlLibrary
{
    using System;
    using System.Linq;
    using System.Drawing;
    using System.Windows.Forms;
    using Library;
    partial class FieldOfTree
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.fieldMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.elementMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.changeImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makeRootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDialog = new System.Windows.Forms.OpenFileDialog();
            this.rootLabel = new System.Windows.Forms.Label();
            this.fieldMenu.SuspendLayout();
            this.elementMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // fieldMenu
            // 
            this.fieldMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem});
            this.fieldMenu.Name = "fieldMenu";
            this.fieldMenu.Size = new System.Drawing.Size(109, 26);
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.createToolStripMenuItem.Text = "Create";
            this.createToolStripMenuItem.Click += new System.EventHandler(this.OnCreateClick);
            // 
            // elementMenu
            // 
            this.elementMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeImageToolStripMenuItem,
            this.makeRootToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.elementMenu.Name = "elementMenu";
            this.elementMenu.Size = new System.Drawing.Size(152, 70);
            // 
            // changeImageToolStripMenuItem
            // 
            this.changeImageToolStripMenuItem.Name = "changeImageToolStripMenuItem";
            this.changeImageToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.changeImageToolStripMenuItem.Text = "Change Image";
            this.changeImageToolStripMenuItem.Click += new System.EventHandler(this.OnChangeImage);
            // 
            // makeRootToolStripMenuItem
            // 
            this.makeRootToolStripMenuItem.Name = "makeRootToolStripMenuItem";
            this.makeRootToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.makeRootToolStripMenuItem.Text = "Make Root";
            this.makeRootToolStripMenuItem.Click += new System.EventHandler(this.OnMakeRootClick);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.OnRemoveClick);
            // 
            // openDialog
            // 
            this.openDialog.FileName = "Image";
            // 
            // rootLabel
            // 
            this.rootLabel.AutoSize = true;
            this.rootLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rootLabel.Location = new System.Drawing.Point(0, 0);
            this.rootLabel.Name = "rootLabel";
            this.rootLabel.Size = new System.Drawing.Size(32, 15);
            this.rootLabel.TabIndex = 2;
            this.rootLabel.Text = "Root";
            // 
            // FieldOfTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.rootLabel);
            this.Name = "FieldOfTree";
            this.Size = new System.Drawing.Size(800, 600);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnFieldRepaint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            this.fieldMenu.ResumeLayout(false);
            this.elementMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip fieldMenu;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip elementMenu;
        private System.Windows.Forms.ToolStripMenuItem changeImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem makeRootToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openDialog;
        private System.Windows.Forms.Label rootLabel;

        public event Action<IFieldElement> ElementCreated;
        public event Action<IFieldElement> ElementRemoved;
        /// <summary>
        /// lastRoot , currentRoot
        /// </summary>
        public event Action<IScene, IScene> RootChanged;
        /// <summary>
        /// lastFactor , currentFactor
        /// </summary>
        public event Action<float, float> ScaleFactorChanged;
        /// <summary>
        /// lastFocus , currentFocus
        /// </summary>
        public event Action<IFieldElement, IFieldElement> FocusChanged;
        /// <summary>
        /// last image, current image
        /// </summary>
        public event Action<IScene, Image, Image> ElementImageChanged;

        public event Action<Control[]> OnStartDrag;
        public event Action<Control[]> OnDrag;
        public event Action<Control[], Point> OnStopDrag;

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            ChangeFocus(null);
            if (e.Button == MouseButtons.Right)
            {
                fieldMenu.Show(MousePosition);
            }
        }
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            ChangeFocus(null);
            if (e.Button == MouseButtons.Left)
            {
                StartDrag();
            }
        }
        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                ScaleFactor += (float)(e.Delta / Math.Abs(e.Delta)) / 50;
            }
        }
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            Dragger.StopDrag();
        }

        private void OnFocusChanged(IFieldElement lastFocus, IFieldElement currentFocus)
        {
            if (currentFocus != null)
                DrawRectNearElement(this.CreateGraphics(), currentFocus);

            if (lastFocus != null)
            {
                var rect = new Rectangle(lastFocus.ScaledLocation.X - 2, lastFocus.ScaledLocation.Y - 2,
                    lastFocus.ScaledSize.Width + 4, lastFocus.ScaledSize.Height + 4);
                Invalidate(rect);
            }
        }

        private void OnElementMouseDown(object sender, MouseEventArgs e)
        {
            if (sender != Focus)
            {
                var lastFocus = Focus;
                ChangeFocus(sender as FieldElement);
                FocusChanged?.Invoke(lastFocus, Focus);
            }
            if (e.Button == MouseButtons.Right)
                elementMenu.Show(MousePosition);
            else if (e.Button == MouseButtons.Left)
                StartDrag();
        }
        private void OnElementMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Dragger.StopDrag();
            }
        }

        private void OnCreateClick(object sender, EventArgs e)
        {
            IFieldElement temp;
            CreateElement(out temp);
            SubscribeTo(temp);
            ElementCreated?.Invoke(temp);
        }

        private void OnChangeImage(object sender, EventArgs e)
        {
            var last_image = Focus.Image;
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Focus.Image = Image.FromFile(openDialog.FileName);
                    ElementImageChanged?.Invoke(Focus, last_image, Focus.Image);
                }
                catch
                {
                    MessageBox.Show("Extension Error");
                }
            }
        }
        private void OnMakeRootClick(object sender, EventArgs e)
        {
            if (Focus != Root)
            {
                var temp = Root;
                SetRoot(Focus);
                RootChanged?.Invoke(temp, Root);
            }
        }
        private void OnRemoveClick(object sender, EventArgs e)
        {
            var temp = Focus;
            RemoveElement(Focus);
            ElementRemoved?.Invoke(temp);
        }
        private void OnDragHandler(Control[] items)
        {
            OnDrag?.Invoke(items);
            UpdateRootMarker();
            Invalidate();
        }

        private void OnStopDragHandler(Control[] elements, Point translateVector)
        {
            if (translateVector.Length() == 0)
                return;
            foreach (IFieldElement item in elements)
            {
                item.ChangeLocation(item.ScaledLocation.Multiply(1 / scaleFactor), scaleFactor);
            }
        }

        private void StartDrag()
        {
            if (tree.Count == 0)
                return;

            var result = from scene in tree.GetAllScenes()
                         where (scene as Control) != null
                         select scene as Control;

            Dragger.OnDrag += OnDragHandler;
            Dragger.OnStartDrag += OnStartDrag;
            Dragger.OnStopDrag += OnStopDrag;

            if (Focus == null)
                Dragger.StartDrag(result.ToArray());
            else
                Dragger.StartDrag(Focus as Control);
        }

        Arrow arrow = new Arrow();
        Arrow highlightArrow = new Arrow(new Pen(Color.Orange, 2), new Pen(Color.Orange, 2));
        Pen rectHighlighter = new Pen(Color.Orange, 1);

        private void DrawRectNearElement(Graphics g, IFieldElement element)
        {
            var pos = element.ScaledLocation;
            var size = element.ScaledSize;
            var rect = new Rectangle(pos.X - 2, pos.Y - 2, size.Width + 3, size.Height + 3);
            g.DrawRectangle(rectHighlighter, rect);
        }

        private void OnFieldRepaint(object sender, PaintEventArgs e)
        {
            if (Focus != null)
            {
                DrawRectNearElement(e.Graphics, Focus);
            }

            foreach (IFieldElement source in tree.GetAllScenes())
            {
                if (HighlightElements.Contains(source))
                    DrawRectNearElement(e.Graphics, source);

                foreach (IFieldElement destination in tree.GetAllLinks(source))
                {
                    if (HighlightElements.Contains(destination))
                        DrawRectNearElement(e.Graphics, destination);

                    Arrow current_arrow = HighlightElements.Contains(source) == HighlightElements.Contains(destination) == true ? highlightArrow : arrow;

                    if (source.Location != destination.Location)
                        current_arrow.Draw(e.Graphics, (source as Control).GetRect(), (destination as Control).GetRect());
                }
            }
        }
    }
}
