using System;
using Library;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WFControlLibrary
{
    public partial class FieldOfTree : UserControl
    {
        public FieldOfTree()
        {
            InitializeComponent();
            MouseWheel += OnMouseWheel;
            UpdateWorldParams();
            OnStopDrag += OnStopDragHandler;
        }

        private Tree tree = new Tree();
        public IFieldElement Root => tree.Root as IFieldElement;

        private List<IFieldElement> HighlightElements = new List<IFieldElement>();

        public new IFieldElement Focus;

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
                    foreach (IFieldElement node in tree.GetAllScenes())
                    {
                        node.ChangeScale(scaleFactor);
                    }
                    Invalidate();
                    ScaleFactorChanged?.Invoke(lastFactor, scaleFactor);
                }
            }
        }

        private FieldOfTree CreateElement()
        {
            IFieldElement temp;
            return CreateElement(out temp);
        }
        private FieldOfTree CreateElement(out IFieldElement element)
        {
            var temp = new FieldElement(PointToClient(MousePosition)) as IFieldElement;
            temp.ChangeLocation(PointToClient(MousePosition).Multiply(1 / scaleFactor), scaleFactor);
            temp.ChangeSize(temp.Size, scaleFactor);
            AddElement(temp);
            element = temp;
            return this;
        }
        private FieldOfTree AddElement(IFieldElement item)
        {
            if (tree.Count == 0)
                item.isRoot = true;
            Controls.Add(item as Control);
            tree.Add(item);
            Focus = item;
            return this;
        }
        private void SubscribeTo(IFieldElement item)
        {
            item.MouseDown += OnElementMouseDown;
            item.MouseUp += OnElementMouseUp;
        }
        private bool RemoveElement(IFieldElement item)
        {
            if (tree.Remove(item) == false)
                return false;

            Controls.Remove(item as Control);

            if (Focus == item)
            {
                Focus = null;
                Invalidate();
            }

            return true;
        }
        public bool AddLink(IFieldElement from, string text, IFieldElement to)
        {
            return tree.AddLink(from, text, to);
        }
        public bool RemoveLink(IFieldElement source, IFieldElement linkedItem)
        {
            return tree.RemoveLink(source, linkedItem);
        }

        public bool RemoveLink(IFieldElement from, string text)
        {
            return tree.RemoveLink(from, text);
        }
        private void SetRoot(IFieldElement item)
        {
            Root.isRoot = false;
            tree.SetRoot(item);
            Root.isRoot = true;
        }
        public IFieldElement[] GetElementsBy(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            var result = from IFieldElement scene in tree.GetAllScenes()
                         where scene.Text.Contains(text)
                         select scene;

            return result.ToArray();
        }

        private void ChangeFocus(IFieldElement item)
        {
            var lastFocus = Focus;
            HighlightElements.Remove(Focus);
            Focus = item;
            FocusChanged?.Invoke(lastFocus, Focus);
        }
        public void GoToPoint(Point worldPoint)
        {
            var offset = worldPoint.Sub(((Point)Size).Multiply(0.5f));

            foreach (IFieldElement item in tree.GetAllScenes())
            {
                item.ChangeLocation(item.Location.Sub(offset.Multiply(1 / scaleFactor)), scaleFactor);
            };

            UpdateWorldParams();
        }
        public void GoToElement(IFieldElement item)
        {
            ChangeFocus(item);
            GoToPoint(item.Location.Add(((Point)item.Size).Multiply(0.5f)));
        }

        private void UpdateWorldParams()
        {
            int _left = 0; int _right = Width; int _up = 0; int _down = Height;

            foreach (Control item in tree.GetAllScenes())
            {
                var rect = item.GetRect();
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

        public void Highlight(IFieldElement item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (HighlightElements.Contains(item))
                return;

            HighlightElements.Add(item);
        }
        public bool RemoveHighlight(IFieldElement item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return HighlightElements.Remove(item);
        }
        public void RemoveAllHighlights()
        {
            HighlightElements = new List<IFieldElement>();
        }

    }
}
