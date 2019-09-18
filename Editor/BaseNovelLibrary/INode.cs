using System.Drawing;

namespace BaseNovelLibrary
{
    public interface INode
    {
        Image Image {get; }
        string Text { get; }

        void ChangeImage(Image @new);
        void ChangeText(string @new);
    }
}
