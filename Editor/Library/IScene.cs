using System.Drawing;

namespace Library
{
    public interface IScene
    {
        Image Image { get; set; }
        string Text { get; set; }
    }
}
