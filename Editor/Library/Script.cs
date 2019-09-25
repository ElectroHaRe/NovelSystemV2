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
            history = new CircularStack<IScene>(tree.Count);

            position = tree.nodes[0];
        }

        private Tree tree;
        private CircularStack<IScene> history;

        private Node position;
        public IScene Current => position.Scene;

        public bool GoNext(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            Node pos;

            if (position.Links.TryGetValue(text, out pos))
            {
                history.Push(Current);
                position = pos;

                return true;
            }
            else return false;
        }

        public bool GoPrevious()
        {
            if (history.Count == 0)
                return false;

            position = tree.GetNodeOf(history.Pop());
            return true;
        }

        public List<IScene> GetHistory()
        {
            var list = new List<IScene>(history.ToArray());

            list.Add(Current);

            return list;
        }

        public void GoTo(IScene scene)
        {
            history.Push(Current);
            position = tree.GetNodeOf(scene);
        }

        public string[] GetLinkedTexts()
        {
            return position.GetLinkedTexts();
        }
    }
}
