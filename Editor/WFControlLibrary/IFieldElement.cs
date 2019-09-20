using System.Drawing;

namespace WFControlLibrary
{
    interface IFieldElement:Library.IScene
    {
        Point Position { get; }
        Size Size { get; }
        bool Focus { get; set; }
        bool Highlight { get; set; }

        void ChangePosition(Point @new);
        void ChangeSize(Size @new);
    }
}
