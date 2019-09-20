using System.Drawing;
using System.Windows.Forms;

namespace WFControlLibrary
{
    using System;

    public interface IFieldElement : Library.IScene
    {
        event Action<IFieldElement, Point, Point, float> LocationChanged;
        event Action<IFieldElement, Size, Size, float> SizeChanged;

        event MouseEventHandler MouseDown;
        event MouseEventHandler MouseUp;

        bool RootMarker { get; set; }

        Point Location { get; }
        Size Size { get; }
        Point ScaledLocation { get; }
        Size ScaledSize { get; }
        bool Focus { get; set; }
        bool Highlight { get; set; }

        void ChangeLocation(Point location, float scale = 1);
        void ChangeSize(Size size, float scale = 1);
        void ChangeScale(float scale);
    }
}
