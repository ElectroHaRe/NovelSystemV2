using System.Windows.Forms;
using System.Drawing;

namespace WFControlLibrary
{
    public static class ControlExtension
    {
        public static Rectangle GetRect(this Control item)
        {
            return new Rectangle(item.Left, item.Right, item.Width, item.Height);
        }
    }
}
