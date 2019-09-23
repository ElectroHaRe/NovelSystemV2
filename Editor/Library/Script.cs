using System;

namespace Library
{
    public class Script
    {
        public Script(Tree tree)
        {
            if (tree.Count == 0)
                throw new ArgumentException("Tree is empty");

            this.tree = tree;
            position = tree.GetNodeOf(tree.Root);
        }

        private Tree tree;

        private LinkedNode position;
        public IScene Current => position.Scene;

        public bool GoNext(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            LinkedNode pos;

            if (position.Links.TryGetValue(text, out pos))
            {
                position = pos;
                return true;
            }
            else return false;
        }

        public void GoTo(IScene scene)
        {
            position = tree.GetNodeOf(scene);
        }

        public string[] GetLinkedTexts()
        {
            return position.GetLinkedTexts();
        }
    }
}
