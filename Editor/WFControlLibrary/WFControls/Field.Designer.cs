namespace WFControlLibrary
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Library;
    partial class Field
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
            this.undoButton = new System.Windows.Forms.Button();
            this.redoButton = new System.Windows.Forms.Button();
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
            // undoButton
            // 
            this.undoButton.Location = new System.Drawing.Point(3, 3);
            this.undoButton.Name = "undoButton";
            this.undoButton.Size = new System.Drawing.Size(75, 23);
            this.undoButton.TabIndex = 2;
            this.undoButton.Text = "Undo";
            this.undoButton.UseVisualStyleBackColor = true;
            this.undoButton.Click += new System.EventHandler(this.OnUndoClick);
            // 
            // redoButton
            // 
            this.redoButton.Location = new System.Drawing.Point(722, 3);
            this.redoButton.Name = "redoButton";
            this.redoButton.Size = new System.Drawing.Size(75, 23);
            this.redoButton.TabIndex = 3;
            this.redoButton.Text = "Redo";
            this.redoButton.UseVisualStyleBackColor = true;
            this.redoButton.Click += new System.EventHandler(this.OnRedoClick);
            // 
            // Field
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.redoButton);
            this.Controls.Add(this.undoButton);
            this.Name = "Field";
            this.Size = new System.Drawing.Size(800, 600);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnFieldRepaint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnElementMouseUp);
            this.fieldMenu.ResumeLayout(false);
            this.elementMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip fieldMenu;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip elementMenu;
        private System.Windows.Forms.ToolStripMenuItem changeImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem makeRootToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openDialog;
        private System.Windows.Forms.Button undoButton;
        private System.Windows.Forms.Button redoButton;

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

        private void OnElementMouseDown(object sender, MouseEventArgs e)
        {
            ChangeFocus(sender as FieldElement);
            if (e.Button == MouseButtons.Right)
                elementMenu.Show(MousePosition);
            else if (e.Button == MouseButtons.Left)
            {
                StartDrag();
            }
        }
        private void OnElementMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ControlDragger.StopDrag();
                UpdateLocations();
            }
        }

        private void OnCreateClick(object sender, EventArgs e)
        {
            var temp = AddElement();
            SubscribeTo(temp);
            ElementCreated?.Invoke(temp);
        }
        private void OnChangeImage(object sender, EventArgs e)
        {
            var last_image = focus.Image;
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Focus.ChangeImage(Image.FromFile(openDialog.FileName));
                    ElementImageChanged?.Invoke(Focus, last_image, focus.Image);
                }
                catch
                {
                    MessageBox.Show("Extension Error");
                }
            }
        }
        private void OnMakeRootClick(object sender, EventArgs e)
        {
            if (focus != Root)
            {
                var temp = Root;
                MakeRoot(Focus);
                RootChanged?.Invoke(temp, Root);
            }
        }
        private void OnRemoveClick(object sender, EventArgs e)
        {
            var temp = RemoveElement(Focus);
            ElementRemoved?.Invoke(temp);
        }
        private void OnDragHandler(Control[] items)
        {
            OnDrag?.Invoke(items);
            Invalidate();
        }

        private void OnUndoClick(object sender, EventArgs e)
        {
            actionStory.Undo();
        }
        private void OnRedoClick(object sender, EventArgs e)
        {
            actionStory.Redo();
        }


        Arrow arrow = new Arrow();
        Arrow highlightArrow = new Arrow(new Pen(Color.Orange, 2), new Pen(Color.Orange, 2));
        Pen rectHighlighter = new Pen(Color.Orange, 1);

        private void DrawRectNearControl(Graphics g, IFieldElement control)
        {
            var pos = control.ScaledLocation;
            var size = control.ScaledSize;
            var rect = new Rectangle(pos.X - 2, pos.Y - 2, size.Width + 3, size.Height + 3);
            g.DrawRectangle(rectHighlighter, rect);
        }

        private void OnFieldRepaint(object sender, PaintEventArgs e)
        {
            if (Focus != null)
            {
                DrawRectNearControl(e.Graphics, focus);
            }
            foreach (IFieldElement source in tree.GetAllScenes())
            {
                if ((source as IFieldElement).Highlight)
                    DrawRectNearControl(e.Graphics, source);

                foreach (IFieldElement destination in tree.GetAllLinkedScenesFor(source))
                {
                    if (destination.Highlight)
                        DrawRectNearControl(e.Graphics, destination);

                    Arrow current_arrow = destination.Highlight == source.Highlight == true ? highlightArrow : arrow;
                    if (source.Location != destination.Location)
                        current_arrow.Draw(e.Graphics, (source as Control).GetRect(), (destination as Control).GetRect());
                }
            }
        }

    }
}
