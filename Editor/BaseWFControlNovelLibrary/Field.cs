using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseNovelLibrary;
using System.Text.RegularExpressions;

namespace BaseWFControlNovelLibrary
{
    public partial class Field : UserControl
    {
        public Field()
        {
            InitializeComponent();
            MouseWheel += MouseWheelHandler;
            UpdateWorldParams();
            ConfigureActionStory();
        }

        private ActionStory actionStory = new ActionStory(20);
        private enum Commands : byte { ElementCreated, ElementRemoved, RootChanged, ElementLocationChanged, ElementImageChanged }
        private void ConfigureActionStory()
        {
            actionStory
                .AddCommand(Commands.ElementCreated.ToString(), (args) => RemoveElement(args[0] as FieldElement), (args) => AddElement(args[0] as FieldElement))
                .AddCommand(Commands.ElementRemoved.ToString(), (args) => AddElement(args[0] as FieldElement), (args) => RemoveElement(args[0] as FieldElement))
                .AddCommand(Commands.RootChanged.ToString(), (args) => MakeRoot(args[0] as FieldElement), (args) => MakeRoot(args[1] as FieldElement))
                .AddCommand(Commands.ElementLocationChanged.ToString(), (args) =>
                {
                    foreach (var item in args[0] as Control[])
                    {
                        item.Location = item.Location.Sub((Point)args[1]);
                    }
                    Invalidate();
                }, (args) =>
                {
                    foreach (var item in args[0] as Control[])
                    {
                        item.Location = item.Location.Add((Point)args[1]);
                    }
                    Invalidate();
                })
                .AddCommand(Commands.ElementImageChanged.ToString(), (args) => (args[0] as INode).ChangeImage(args[1] as Image),
                (args) => (args[0] as INode).ChangeImage(args[2] as Image));

            ElementCreated += (element) => actionStory.Store(Commands.ElementCreated.ToString(), element);
            ElementRemoved += (element) => actionStory.Store(Commands.ElementRemoved.ToString(), element);
            RootChanged += (lastRoot, currentRoot) => actionStory.Store(Commands.RootChanged.ToString(), lastRoot, currentRoot);
            OnStopDrag += (elements, translateVector) => actionStory.Store(Commands.ElementLocationChanged.ToString(), elements, translateVector);
            ElementImageChanged += (element, lastImage, currentImage) => actionStory.Store(Commands.ElementImageChanged.ToString(), element, lastImage, currentImage);
        }

        public event Action<FieldElement> ElementCreated;
        public event Action<FieldElement> ElementRemoved;
        /// <summary>
        /// lastRoot , currentRoot
        /// </summary>
        public event Action<FieldElement, FieldElement> RootChanged;
        /// <summary>
        /// lastFactor , currentFactor
        /// </summary>
        public event Action<float, float> ScaleFactorChanged;
        /// <summary>
        /// lastFocus , currentFocus
        /// </summary>
        public event Action<FieldElement, FieldElement> FocusChanged;
        /// <summary>
        /// last image, current image
        /// </summary>
        public event Action<FieldElement, Image, Image> ElementImageChanged;

        public event Action<Control[]> OnStartDrag;
        public event Action<Control[]> OnDrag;
        public event Action<Control[], Point> OnStopDrag;

        private NodalTree tree = new NodalTree();
        public FieldElement Root => tree?.Root as FieldElement;

        private FieldElement focus;
        public new FieldElement Focus
        {
            get => focus;
            set
            {
                if (focus != null)
                    (focus as IFieldElement).Highlight = false;
                var lastFocus = focus;
                focus = value;
                if (focus != null)
                    (focus as IFieldElement).Highlight = true;
                FocusChanged?.Invoke(lastFocus, Focus);
                Invalidate();
            }
        }

        public Size VisibleFieldSize => Size;
        public Size WorldSize { get; private set; }
        public Point WorldCenterPoint { get; private set; }

        private float scaleFactor = 0.5f;
        public float ScaleFactor
        {
            get => scaleFactor;
            set
            {
                if (value != scaleFactor)
                {
                    var lastFactor = scaleFactor;
                    scaleFactor = value < 0.12f ? 0.12f : value > 1 ? 1 : value;
                    INode[] nodes = tree.GetAllNodes();
                    for (int i = 0; i < nodes?.Length; i++)
                    {
                        var a = (nodes[i] as FieldElement).Location;
                        (nodes[i] as FieldElement).Location = (nodes[i] as FieldElement).RealPosition.Multiply(scaleFactor).Add(((Point)Size).Multiply(0.5f));
                        (nodes[i] as FieldElement).Size = (Size)((Point)(nodes[i] as FieldElement).RealSize).Multiply(scaleFactor);
                    }
                    Invalidate();
                    ScaleFactorChanged?.Invoke(lastFactor, scaleFactor);
                }
            }
        }

        private FieldElement CreateElement()
        {
            var temp = new FieldElement(PointToClient(MousePosition));
            temp.RealPosition = temp.Location.Sub(((Point)Size).Multiply(0.5f)).Multiply(1 / scaleFactor);
            temp.Size = (Size)((Point)temp.Size).Multiply(scaleFactor);
            temp.MouseDown += ElementMouseDownHandler;
            temp.MouseUp += ElementMouseUpHandler;
            AddElement(temp);
            return temp;
        }
        private FieldElement AddElement(FieldElement @new)
        {
            if (tree.Count == 0)
                @new.rootMarker = true;
            Controls.Add(@new);
            tree.AddNode(@new);
            tree.GoToNode(@new);
            focus = @new;
            return @new;
        }
        private FieldElement RemoveElement(FieldElement item)
        {
            tree.RemoveNode(item);
            Controls.Remove(item);
            if (Focus == item)
                Focus = null;
            if (tree.Root != null && !(tree.Root as FieldElement).rootMarker)
                (tree.Root as FieldElement).rootMarker = true;
            return item;
        }
        private void MakeRoot(FieldElement item)
        {
            (tree.Root as FieldElement).rootMarker = false;
            tree.ChangeRoot(item);
            (tree.Root as FieldElement).rootMarker = true;
        }

        #region Handlers
        private void MouseClickHandler(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                fieldMenu.Show(MousePosition);
            }
            Focus = null;
        }
        private void MouseDownHandler(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Focus = null;
                if (tree.Count == 0)
                    return;
                INode[] nodes = tree.GetAllNodes();
                Control[] controls = new Control[tree.Count];
                Parallel.For(0, nodes.Length, i => controls[i] = nodes[i] as Control);
                ControlDragger.OnDrag += DragHandler;
                ControlDragger.OnStartDrag += OnStartDrag;
                ControlDragger.OnStopDrag += OnStopDrag;
                ControlDragger.StartDrag(controls);
            }
        }
        private void MouseWheelHandler(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                ScaleFactor += (float)(e.Delta / Math.Abs(e.Delta)) / 50;
            }
        }

        private void DragHandler(Control[] items)
        {
            OnDrag?.Invoke(items);
            Invalidate();
        }

        private void ElementMouseDownHandler(object sender, MouseEventArgs e)
        {
            INode temp = sender as INode;
            tree.GoToNode(temp);
            Focus = tree.CurrentNode as FieldElement;
            if (e.Button == MouseButtons.Right)
                elementMenu.Show(MousePosition);
            else if (e.Button == MouseButtons.Left)
            {
                ControlDragger.OnDrag += DragHandler;
                ControlDragger.OnStartDrag += OnStartDrag;
                ControlDragger.OnStopDrag += OnStopDrag;
                ControlDragger.StartDrag(Focus);
            }
        }
        private void ElementMouseUpHandler(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ControlDragger.StopDrag();
                UpdateRealPositions();
            }
        }

        private void CreateClickHandler(object sender, EventArgs e)
        {
            var temp = CreateElement();
            ElementCreated?.Invoke(temp);
        }
        private void ChangeImageClickHandler(object sender, EventArgs e)
        {
            var last_image = focus.Image;
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Focus.Image = Image.FromFile(openDialog.FileName);
                    ElementImageChanged?.Invoke(Focus, last_image, focus.Image);
                }
                catch
                {
                    MessageBox.Show("Extension Error");
                }
            }
        }
        private void MakeRootClickHandler(object sender, EventArgs e)
        {
            var temp = Root;
            MakeRoot(Focus);
            RootChanged?.Invoke(temp, Root);
        }
        private void RemoveClickHandler(object sender, EventArgs e)
        {
            var temp = RemoveElement(Focus);
            ElementRemoved?.Invoke(temp);
        }
        #endregion

        private void UpdateWorldParams()
        {
            int _left = 0; int _right = Width; int _up = 0; int _down = Height;
            Parallel.ForEach(tree.GetAllNodes(), item =>
            {
                {
                    var temp = item as FieldElement;
                    if (temp.Left < _left)
                        _left = temp.Left;
                    if (temp.Left + temp.Width > _right)
                        _right = temp.Left + temp.Width;
                    if (temp.Top < _up)
                        _up = temp.Top;
                    if (temp.Top + temp.Height > _down)
                        _down = temp.Top + temp.Height;
                }
            });
            var _main_factor = (float)Width / Height;
            Size _size = new Size(_right - _left, _down - _up);
            var _new_factor = (float)_size.Width / _size.Height;
            if (_main_factor > _new_factor)
            {
                _size.Width = (int)(_size.Height / _main_factor);
            }
            else
            {
                _size.Height = (int)(_size.Width * _main_factor);
            }
            WorldSize = _size;
            WorldCenterPoint = new Point(_left + _size.Width / 2, _up + _size.Height / 2);
        }
        private void UpdateRealPositions()
        {
            Parallel.ForEach(tree.GetAllNodes(), item =>
            {
                (item as FieldElement).RealPosition = (item as FieldElement).Location.Sub(((Point)Size).Multiply(0.5f)).Multiply(1 / scaleFactor);
            });
        }

        public void FocusOnPoint(Point worldPoint)
        {
            var offset = worldPoint.Sub(((Point)Size).Multiply(0.5f));

            foreach (var item in tree.GetAllNodes())
            {
                (item as FieldElement).Location = (item as FieldElement).Location.Sub(offset);
            };
            UpdateRealPositions();
            UpdateWorldParams();
        }

        public void SaveTree(string path)
        {
            using (System.IO.Stream stream = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate))
            {
                var serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                serializer.Serialize(stream, tree);
            }
        }
        public void LoadTree(string path)
        {
            using (System.IO.Stream stream = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate))
            {
                var serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                tree = serializer.Deserialize(stream) as NodalTree;
                foreach (var item in tree.GetAllNodes())
                {
                    (item as FieldElement).Location = (item as FieldElement).RealPosition.Multiply(scaleFactor).Add(((Point)Size).Multiply(0.5f));
                    (item as FieldElement).Size = (Size)((Point)(item as FieldElement).RealSize).Multiply(scaleFactor);
                    (item as FieldElement).MouseDown += ElementMouseDownHandler;
                    (item as FieldElement).MouseUp += ElementMouseUpHandler;
                    Controls.Add(item as FieldElement);
                }
            }
        }

        #region TreeWrapper
        public void AddTie(string text, FieldElement item)
        {
            tree.AddTie(text, item);
        }
        /// <summary>
        /// Удаляет связь элемента в фокусе с указанным элементом
        /// </summary>
        /// <param name="item">Элемент, сязь с которым следует устранить</param>
        public void RemoveTie(FieldElement item)
        {
            tree.RemoveTie(item);
        }
        /// <summary>
        /// Удаляет связь элемента в фокусе по тексту
        /// </summary>
        /// <param name="text">Текст связи</param>
        public void RemoveTie(string text)
        {
            tree.RemoveTie(text);
        }
        public void RemoveTie(FieldElement from, FieldElement to)
        {
            tree.RemoveTie(from, to);
        }
        public void FocusOnElement(FieldElement item)
        {
            tree.GoToNode(item);
            Focus = item;
            FocusOnPoint(item.Location.Add(((Point)item.Size).Multiply(0.5f)));
        }
        public FieldElement[] FindElements(string text)
        {
            INode[] nodes = tree.GetAllNodes() ?? new INode[0];
            if (nodes.Length == 0)
                return new FieldElement[0];
            System.Collections.Generic.List<FieldElement> elements = new System.Collections.Generic.List<FieldElement>();
            for (int i = 0; i < nodes.Length; i++)
            {
                if (Regex.IsMatch(nodes[i].Text, "(" + text + ")+"))
                    elements.Add(nodes[i] as FieldElement);
            }
            return elements.ToArray();
        }
        #endregion

        public void RemoveAllHighlights()
        {
            Parallel.ForEach(tree.GetAllNodes(), item => (item as IFieldElement).Highlight = false);
        }

        Arrow arrow = new Arrow();
        Arrow highlightArrow = new Arrow(new Pen(Color.Orange, 2), new Pen(Color.Orange, 2));
        Pen rectHighlighter = new Pen(Color.Orange, 1);

        private void DrawHighlightOf(Graphics g, FieldElement fieldElement)
        {
            var pos = fieldElement.Location;
            var size = fieldElement.Size;
            var rect = new Rectangle(pos.X - 2, pos.Y - 2, size.Width + 3, size.Height + 3);
            g.DrawRectangle(rectHighlighter, rect);
        }
        private void Field_Paint(object sender, PaintEventArgs e)
        {
            if (Focus != null)
            {
                DrawHighlightOf(e.Graphics, focus);
            }
            foreach (var source in tree.GetAllNodes())
            {
                if ((source as IFieldElement).Highlight)
                    DrawHighlightOf(e.Graphics, source as FieldElement);

                foreach (var destination in tree.GetAllNextNodes(source))
                {
                    if ((destination as IFieldElement).Highlight)
                        DrawHighlightOf(e.Graphics, destination as FieldElement);

                    Arrow current_arrow = (destination as IFieldElement).Highlight == (source as IFieldElement).Highlight == true ? highlightArrow : arrow;
                    if ((source as FieldElement).Location != (destination as FieldElement).Location)
                        current_arrow.Draw(e.Graphics, source as FieldElement, destination as FieldElement);
                }
            }
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            actionStory.Undo();
        }
        private void RedoButton_Click(object sender, EventArgs e)
        {
            actionStory.Redo();
        }
    }
}
