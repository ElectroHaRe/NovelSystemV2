using System;
using System.Runtime.Serialization;
using System.Drawing;
using System.Windows.Forms;
using Library;

namespace WFControlLibrary
{
    [Serializable]
    public partial class FieldElement : UserControl, IFieldElement, ISerializable
    {
        event Action<IFieldElement, Point, Point, float> _locationChanged;
        event Action<IFieldElement, Point, Point, float> IFieldElement.LocationChanged
        {
            add { _locationChanged += value; }
            remove { _locationChanged -= value; }
        }

        event Action<IFieldElement, Size, Size, float> _sizeChanged;
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

        bool IFieldElement.RootMarker { get => rootMarker; set => rootMarker = value; }

        Image IScene.Image => pictureBox.Image;
        string IScene.Text => textBox.Text;

        private Point _location = new Point();
        Point IFieldElement.Location => _location;

        private Size _size = new Size();
        Size IFieldElement.Size => _size;

        Point IFieldElement.ScaledLocation => this.Location;

        Size IFieldElement.ScaledSize => this.Size;

        bool IFieldElement.Focus { get; set; }
        bool IFieldElement.Highlight { get; set; }

        void IScene.ChangeImage(Image @new)
        {
            pictureBox.Image = @new;
            if (@new == null)
                noImageLabel.Enabled = noImageLabel.Visible = true;
            else noImageLabel.Enabled = noImageLabel.Visible = false;
            ImageChanged?.Invoke(pictureBox, new EventArgs());
        }
        void IScene.ChangeText(string @new) => textBox.Text = @new;

        void IFieldElement.ChangeLocation(Point location, float scale = 1)
        {
            var lastLocation = _location;
            _location = location;
            var l = location.Length();
            var normal = location.Normalize();
            this.Location = normal.Multiply(l*scale).ToPoint();
            _locationChanged?.Invoke(this, lastLocation, _location, scale);
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
            (this as IScene).ChangeImage(info.GetValue("image", typeof(Image)) as Image);
            (this as IScene).ChangeText(info.GetValue("text", typeof(string)) as string);
            (this as IFieldElement).ChangeLocation((Point)info.GetValue("position", typeof(Point)));
            (this as IFieldElement).ChangeSize((Size)info.GetValue("size", typeof(Size)));
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
