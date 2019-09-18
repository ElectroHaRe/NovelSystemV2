using System.Drawing;

namespace BaseWFControlNovelLibrary
{
    interface IFieldElement:BaseNovelLibrary.INode
    {
        Point Position { get; }
        Size Size { get; }
        bool Focus { get; set; }
        bool Highlight { get; set; }

        void ChangePosition(Point @new);
        void ChangeSize(Size @new);
    }
}
