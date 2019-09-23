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
            nodes.Add(new LinkedNode(root));
            root.isRoot = false;
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
        private List<LinkedNode> nodes = new List<LinkedNode>();

        public int Count => nodes.Count;

        internal LinkedNode GetNodeOf(IScene scene)
        {
            if (scene == null)
                throw new ArgumentNullException("scene");

            var node = nodes.Find((item) => item.Scene == scene);

            if (node == null)
                throw new Exception("Scene not fount");

            return node;
        }

        public bool Add(IScene scene)
        {
            if (scene == null)
                throw new ArgumentNullException();

            try
            {
                GetNodeOf(scene);
            }
            catch
            {
                nodes.Add(new LinkedNode(scene));
                if (nodes.Count > 1)
                    scene.isRoot = false;
                else scene.isRoot = true;
                return true;
            }

            return false;
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
                if (nodes.Count > 0 && nodes[0].Scene.isRoot == false)
                    nodes[0].Scene.isRoot = true;
                return true;
            }

            return false;
        }
        public void SetRoot(IScene scene)
        {
            var node = GetNodeOf(scene);

            nodes[0].Scene.isRoot = false;

            nodes[nodes.IndexOf(node)] = nodes[0];
            nodes[0] = node;

            nodes[0].Scene.isRoot = true;
        }

        public IScene[] GetAllScenes()
        {
            var scenes = from node in nodes
                         select node.Scene;
            return scenes.ToArray();
        }

        public bool AddLink(IScene from, string text, IScene to)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            var source = GetNodeOf(from);
            var destination = GetNodeOf(to);

            return source.AddLink(text, destination);
        }
        public bool RemoveLink(IScene from, IScene to)
        {
            if (to == null)
                throw new ArgumentNullException("to");

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
