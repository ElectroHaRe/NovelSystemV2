using System;
using System.Collections.Generic;

namespace Library
{
    public class Script
    {
        public Script(Tree tree)
        {
            if (tree.Count == 0)
                throw new ArgumentException("Tree is empty");

            this.tree = tree;
            Story = new CircularStack<IScene>(tree.Count);

            position = tree.GetNodeOf(tree.Root);
        }

        private Tree tree;
        private CircularStack<IScene> Story;

        private LinkedNode position;
        public IScene Current => position.Scene;

        public bool GoNext(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            LinkedNode pos;

            if (position.Links.TryGetValue(text, out pos))
            {
                Story.Push(Current);
                position = pos;

                return true;
            }
            else return false;
        }

        public bool GoPrevious()
        {
            if (Story.Count == 0)
                return false;

            position = tree.GetNodeOf(Story.Pop());
            return true;
        }

        public List<IScene> GetStory()
        {
            var list = new List<IScene>();

            list.AddRange(Story.ToArray());
            list.Add(Current);

            return list;
        }

        public void GoTo(IScene scene)
        {
            Story.Push(Current);
            position = tree.GetNodeOf(scene);
        }

        public string[] GetLinkedTexts()
        {
            return position.GetLinkedTexts();
        }
    }
}
