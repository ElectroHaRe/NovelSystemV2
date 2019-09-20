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
        public FieldElement()
        {
            InitializeComponent();
            RealPosition = this.Location;
            RealSize = this.Size;
            rootMarker = false;
        }

        public FieldElement(Point location) : this()
        {
            Location = location;
        }

        Image IScene.Image => pictureBox.Image;
        string IScene.Text => textBox.Text;

        private Point _position = new Point();
        Point IFieldElement.Position => _position;
        private Size _size = new Size();
        Size IFieldElement.Size => _size;

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

        void IFieldElement.ChangePosition(Point @new) { _position = @new; RealPositionChanged?.Invoke(this, new EventArgs()); }
        void IFieldElement.ChangeSize(Size @new) { _size = @new; RealSizeChanged?.Invoke(this, new EventArgs()); }

        protected FieldElement(SerializationInfo info, StreamingContext context) : this()
        {
            (this as IScene).ChangeImage(info.GetValue("image", typeof(Image)) as Image);
            (this as IScene).ChangeText(info.GetValue("text", typeof(string)) as string);
            (this as IFieldElement).ChangePosition((Point)info.GetValue("position", typeof(Point)));
            (this as IFieldElement).ChangeSize((Size)info.GetValue("size", typeof(Size)));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("image", pictureBox.Image);
            info.AddValue("text", Text);
            info.AddValue("position", _position);
            info.AddValue("size", _size);
        }
    }
}
