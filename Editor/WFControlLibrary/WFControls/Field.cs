using System;
using Library;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WFControlLibrary
{
    public partial class Field : UserControl
    {
        public Field()
        {
            InitializeComponent();
            MouseWheel += OnMouseWheel;
            UpdateWorldParams();
            ConfigureActionStory();
        }

        private ActionStory actionStory = new ActionStory(200);
        private enum Commands : byte
        {
            ElementCreated,
            ElementRemoved,
            RootChanged,
            ElementLocationChanged,
            ElementImageChanged
        }
        private void ConfigureActionStory()
        {
            actionStory
                .AddCommand(Commands.ElementCreated.ToString(), (args) => RemoveElement(args[0] as IFieldElement), (args) => AddElement(args[0] as IFieldElement))
                .AddCommand(Commands.ElementRemoved.ToString(), (args) => AddElement(args[0] as IFieldElement), (args) => RemoveElement(args[0] as IFieldElement))
                .AddCommand(Commands.RootChanged.ToString(), (args) => MakeRoot(args[0] as IFieldElement), (args) => MakeRoot(args[1] as IFieldElement))
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
                .AddCommand(Commands.ElementImageChanged.ToString(), (args) => (args[0] as IScene).ChangeImage(args[1] as Image),
                (args) => (args[0] as IScene).ChangeImage(args[2] as Image));

            ElementCreated += (element) => actionStory.Store(Commands.ElementCreated.ToString(), element);
            ElementRemoved += (element) => actionStory.Store(Commands.ElementRemoved.ToString(), element);
            RootChanged += (lastRoot, currentRoot) => actionStory.Store(Commands.RootChanged.ToString(), lastRoot, currentRoot);
            OnStopDrag += (elements, translateVector) => actionStory.Store(Commands.ElementLocationChanged.ToString(), elements, translateVector);
            ElementImageChanged += (element, lastImage, currentImage) => actionStory.Store(Commands.ElementImageChanged.ToString(), element, lastImage, currentImage);
        }

        private Tree tree = new Tree();
        public IFieldElement Root => tree?.Root as IFieldElement;

        private IFieldElement focus;
        public new IFieldElement Focus
        {
            get => focus;
            set
            {
                if (focus != null)
                    focus.Highlight = false;
                var lastFocus = focus;
                focus = value;
                if (focus != null)
                    focus.Highlight = true;
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
                var lastFactor = scaleFactor;
                scaleFactor = value < 0.12f ? 0.12f : value > 1 ? 1 : value;
                if (lastFactor != scaleFactor)
                {
                    IScene[] nodes = tree.GetAllScenes();
                    for (int i = 0; i < nodes?.Length; i++)
                    {
                        var node = nodes[i] as IFieldElement;

                        var halfSize = ((Point)Size).Multiply(0.5f);
                        var offsetVector = node.ScaledLocation.Sub(halfSize).Multiply(scaleFactor / lastFactor);
                        var scaledLocation = halfSize.Add(offsetVector);
                        var location = scaledLocation.Multiply(1 / scaleFactor);

                        node.ChangeLocation(location, scaleFactor);
                        node.ChangeSize(node.Size, scaleFactor);
                    }
                    Invalidate();
                    ScaleFactorChanged?.Invoke(lastFactor, scaleFactor);
                }
            }
        }

        private IFieldElement AddElement()
        {
            var temp = new FieldElement() as IFieldElement;
            temp.ChangeLocation(PointToClient(MousePosition).Multiply(1 / scaleFactor), scaleFactor);
            temp.ChangeSize(temp.Size, scaleFactor);
            AddElement(temp);
            return temp;
        }
        private IFieldElement AddElement(IFieldElement item)
        {
            if (tree.Count == 0)
                item.RootMarker = true;
            Controls.Add(item as Control);
            tree.AddScene(item);
            tree.SetCurrent(item);
            focus = item;
            return item;
        }
        private IFieldElement SubscribeTo(IFieldElement item)
        {
            item.MouseDown += OnElementMouseDown;
            item.MouseUp += OnElementMouseUp;
            return item;
        }
        private IFieldElement RemoveElement(IFieldElement item)
        {
            tree.Remove(item);
            Controls.Remove(item as Control);
            if (Focus == item)
                Focus = null;
            if (tree.Root != null && !Root.RootMarker)
                Root.RootMarker = true;
            return item;
        }
        public void AddLink(IFieldElement from, string text, IFieldElement to)
        {
            tree.AddLink(from, text, to);
        }
        public void RemoveLink(IFieldElement source, IFieldElement linkedItem)
        {
            tree.RemoveLink(source, linkedItem);
        }
        public void RemoveLink(IFieldElement source, string linkedText)
        {
            tree.RemoveLink(source, linkedText);
        }
        private void MakeRoot(IFieldElement item)
        {
            Root.RootMarker = false;
            tree.ChangeRoot(item);
            Root.RootMarker = true;
        }
        public IFieldElement[] FindElements(string text)
        {
            IScene[] nodes = tree.GetAllScenes() ?? new IScene[0];
            if (nodes.Length == 0)
                return new IFieldElement[0];
            System.Collections.Generic.List<IFieldElement> elements = new System.Collections.Generic.List<IFieldElement>();
            for (int i = 0; i < nodes.Length; i++)
            {
                if (Regex.IsMatch(nodes[i].Text, "(" + text + ")+"))
                    elements.Add(nodes[i] as IFieldElement);
            }
            return elements.ToArray();
        }

        private void ChangeFocus(IFieldElement item)
        {
            if (item == null)
            {
                Focus = null;
                return;
            }
            tree.SetCurrent(item);
            Focus = tree.CurrentScene as IFieldElement;
        }
        public void GoToPoint(Point worldPoint)
        {
            var offset = worldPoint.Sub(((Point)Size).Multiply(0.5f));

            foreach (IFieldElement item in tree.GetAllScenes())
            {
                item.ChangeLocation(item.Location.Sub(offset.Multiply(1 / scaleFactor)), scaleFactor);
            };
            UpdateLocations();
            UpdateWorldParams();
        }
        public void GoToElement(IFieldElement item)
        {
            tree.SetCurrent(item);
            Focus = item;
            GoToPoint(item.Location.Add(((Point)item.Size).Multiply(0.5f)));
        }

        private void StartDrag()
        {
            if (tree.Count == 0)
                return;
            Control[] controls;
            if (focus == null)
            {
                IScene[] nodes = tree.GetAllScenes();
                controls = new Control[tree.Count];
                Parallel.For(0, nodes.Length, i => controls[i] = nodes[i] as Control);
            }
            else controls = new Control[1] { focus as Control };
            ControlDragger.OnDrag += OnDragHandler;
            ControlDragger.OnStartDrag += OnStartDrag;
            ControlDragger.OnStopDrag += OnStopDrag;
            ControlDragger.StartDrag(controls);
        }

        private void UpdateWorldParams()
        {
            int _left = 0; int _right = Width; int _up = 0; int _down = Height;
            foreach (IFieldElement item in tree.GetAllScenes())
            {
                var rect = new Rectangle(item.Location.X, item.Location.Y, item.Size.Width, item.Size.Height);
                if (rect.Left < _left)
                    _left = rect.Left;
                if (rect.Left + rect.Width > _right)
                    _right = rect.Left + rect.Width;
                if (rect.Top < _up)
                    _up = rect.Top;
                if (rect.Top + rect.Height > _down)
                    _down = rect.Top + rect.Height;
            }
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
        private void UpdateLocations()
        {
            foreach (IFieldElement item in tree.GetAllScenes())
            {
                item.ChangeLocation(item.ScaledLocation.Multiply(1 / scaleFactor), scaleFactor);
            }
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
                tree = serializer.Deserialize(stream) as Tree;
                foreach (IFieldElement item in tree.GetAllScenes())
                {
                    item.ChangeLocation(item.Location.Add(((Point)Size).Multiply(0.5f).Multiply(1 / scaleFactor)), scaleFactor);
                    item.ChangeSize(item.Size, scaleFactor);
                    item.MouseDown += OnElementMouseDown;
                    item.MouseUp += OnElementMouseUp;
                    Controls.Add(item as Control);
                }
            }
        }

        public void RemoveAllHighlights()
        {
            Parallel.ForEach(tree.GetAllScenes(), item => (item as IFieldElement).Highlight = false);
        }
    }
}
