using System.Drawing;

namespace Library
{
    public interface IScene
    {
        Image Image { get; }
        string Text { get; }

        void ChangeImage(Image @new);
        void ChangeText(string @new);
    }
}
