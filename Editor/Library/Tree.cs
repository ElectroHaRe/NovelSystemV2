using System;
using System.Linq;
using System.Collections.Generic;

namespace Library
{
    [Serializable]
    public class Tree
    {
        public Tree() { }
        public Tree(IScene root)
        {
            nodes.Add(new Node(root));
        }

        public IScene Root
        {
            get
            {
                if (nodes.Count == 0)
                    return null;
                return nodes[0].Scene;
            }
        }
        private List<Node> nodes = new List<Node>();

        public int Count => nodes.Count;

        internal Node GetNodeOf(IScene scene)
        {
            if (scene == null)
                throw new ArgumentNullException("scene");

            var node = nodes.Find((item) => item.Scene == scene);

            if (node == null)
                throw new Exception("Scene not fount");

            return node;
        }

        internal bool Contains(IScene scene)
        {
            if (scene == null)
                throw new ArgumentNullException("scene");

            if (nodes.Find((item) => item.Scene == scene) != null)
                return true;
            else return false;
        }

        public bool Add(IScene scene)
        {
            if (scene == null)
                throw new ArgumentNullException();

            if (Contains(scene))
                return false;

            nodes.Add(new Node(scene));
            return true;
        }
        public bool Remove(IScene scene)
        {
            var node = GetNodeOf(scene);

            foreach (var item in nodes)
            {
                item.RemoveLink(scene);
            }

            if (nodes.Remove(node))
            {
                return true;
            }
            else return false;
        }
        public void SetRoot(IScene scene)
        {
            var node = GetNodeOf(scene);

            nodes[nodes.IndexOf(node)] = nodes[0];
            nodes[0] = node;
        }

        public IScene[] GetAllScenes()
        {
            var scenes = from node in nodes
                         select node.Scene;

            return scenes.ToArray();
        }

        public bool AddLink(IScene from, string text, IScene to)
        {
            var source = GetNodeOf(from);
            var destination = GetNodeOf(to);

            return source.AddLink(text, destination);
        }
        public bool RemoveLink(IScene from, IScene to)
        {
            var source = GetNodeOf(from);

            return source.RemoveLink(to);
        }
        public bool RemoveLink(IScene from, string text)
        {
            var node = GetNodeOf(from);

            return node.RemoveLink(text);
        }
        public IScene[] GetAllLinks(IScene scene)
        {
            var result = from link in GetNodeOf(scene).Links
                         select link.Value.Scene;

            return result.ToArray();
        }

        public Script GetScript()
        {
            return new Script(this);
        }
    }
}
