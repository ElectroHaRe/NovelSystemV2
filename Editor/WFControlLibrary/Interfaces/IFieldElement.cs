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

        Point Location { get; }
        Size Size { get; }
        Point ScaledLocation { get; }
        Size ScaledSize { get; }

        void ChangeLocation(PointF location, float scale = 1);
        void ChangeSize(Size size, float scale = 1);
        void ChangeScale(float scale);
    }
}
