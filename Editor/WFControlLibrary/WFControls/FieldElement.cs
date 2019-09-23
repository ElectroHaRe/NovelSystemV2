using System;
using Library;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace WFControlLibrary
{
    [Serializable]
    public partial class FieldElement : UserControl, IFieldElement, ISerializable
    {
        private event Action<IFieldElement, Point, Point, float> _locationChanged;
        event Action<IFieldElement, Point, Point, float> IFieldElement.LocationChanged
        {
            add { _locationChanged += value; }
            remove { _locationChanged -= value; }
        }

        private event Action<IFieldElement, Size, Size, float> _sizeChanged;
        event Action<IFieldElement, Size, Size, float> IFieldElement.SizeChanged
        {
            add { _sizeChanged += value; }
            remove { _sizeChanged -= value; }
        }

        event MouseEventHandler IFieldElement.MouseDown
        {
            add { MouseDown += value; }
            remove { MouseDown -= value; }
        }
        event MouseEventHandler IFieldElement.MouseUp
        {
            add { MouseUp += value; }
            remove { MouseDown -= value; }
        }

        public FieldElement()
        {
            InitializeComponent();
            _location = this.Location;
            _size = this.Size;
            rootMarker = false;
        }

        public FieldElement(Point location) : this()
        {
            Location = location;
        }

        Image IScene.Image
        {
            get => pictureBox.Image;
            set
            {
                var lastImage = value;
                pictureBox.Image = value;
                noImageLabel.Enabled = noImageLabel.Visible = (value == null);
                ImageChanged?.Invoke(pictureBox, lastImage, value);
            }
        }
        string IScene.Text
        {
            get => textBox.Text;
            set => textBox.Text = value;
        }

        private Point _location;
        Point IFieldElement.Location => _location;

        private Size _size;
        Size IFieldElement.Size => _size;

        Point IFieldElement.ScaledLocation => this.Location;

        Size IFieldElement.ScaledSize => this.Size;

        bool IScene.isRoot
        {
            get => rootLabel.Enabled == rootLabel.Visible == true;
            set => rootLabel.Enabled = rootLabel.Visible = value;
        }

        void IFieldElement.ChangeLocation(PointF location, float scale = 1)
        {
            var lastLocation = (PointF)_location;
            _location = location.ToPoint();
            double length = location.Length() * scale;
            this.Location = location.ChangeLength(length).ToPoint();
            _locationChanged?.Invoke(this, lastLocation.ToPoint(), _location, scale);
        }
        void IFieldElement.ChangeSize(Size size, float scale)
        {
            var lastSize = _size;
            _size = size;
            this.Size = (Size)((Point)size).Multiply(scale);
            _sizeChanged?.Invoke(this, lastSize, _size, scale);
        }

        void IFieldElement.ChangeScale(float scale)
        {
            var temp = (this as IFieldElement);
            temp.ChangeLocation(temp.Location, scale);
            temp.ChangeSize(temp.Size, scale);
        }

        protected FieldElement(SerializationInfo info, StreamingContext context) : this()
        {
            var temp = this as IFieldElement;
            temp.Image = (info.GetValue("image", typeof(Image)) as Image);
            temp.Text = (info.GetValue("text", typeof(string)) as string);
            temp.ChangeLocation((Point)info.GetValue("position", typeof(Point)));
            temp.ChangeSize((Size)info.GetValue("size", typeof(Size)));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("image", pictureBox.Image);
            info.AddValue("text", Text);
            info.AddValue("position", _location);
            info.AddValue("size", _size);
        }
    }
}
