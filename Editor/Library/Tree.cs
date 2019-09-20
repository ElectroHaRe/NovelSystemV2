using System;
using System.Linq;
using System.Collections.Generic;

namespace Library
{
    [Serializable]
    public class Tree
    {
        public class Iterator
        {
            public Iterator(Tree tree)
            {
                this.tree = tree;
            }

            private Tree tree;

            public IScene GoNext(string text)
            {
                tree.position = tree.position?.Next(text);
                return tree.CurrentScene;
            }

            public IScene GoPrevious()
            {
                tree.position = tree.position?.Previous;
                return tree.CurrentScene;
            }
        }

        ArgumentException SceneAlreadyExists = new ArgumentException("Scene already exists");
        ArgumentException SceneDoesNotExist = new ArgumentException("Scene does not exist");

        public Tree() { iterator = new Iterator(this); }
        public Tree(IScene root) : this()
        {
            nodes.Add(new Node(root));
            position = nodes[0];
        }

        public IScene this[int index] => GetAllScenes()[index];

        private Node position;
        private List<Node> nodes = new List<Node>();

        public IScene Root => nodes.Count > 0 ? nodes[0].Scene : null;
        public IScene CurrentScene => position?.Scene;

        public int Count => nodes.Count;

        private Iterator iterator;

        public Iterator GetIterator() => iterator;

        private Node GetNodeOf(IScene scene)
        {
            return nodes.Find((node) => node.Scene == scene);
        }

        public void AddScene(IScene scene)
        {
            if (GetNodeOf(scene) != null)
                throw SceneAlreadyExists;
            if (scene == null)
                throw new ArgumentNullException();

            nodes.Add(new Node(scene));

            if (nodes.Count == 1)
                position = nodes[0];
        }
        public void AddLink(IScene from, string text, IScene to)
        {
            if (from == null || to == null)
                throw new ArgumentNullException();

            var node = GetNodeOf(from);

            if (node == null)
                throw SceneDoesNotExist;

            var destination = nodes.Find((item) => item.Scene == to) ?? new Node(to);

            node.AddLink(text, destination);
        }

        public void ChangeRoot(IScene root)
        {
            if (root == null)
                throw new ArgumentNullException();

            Node node = GetNodeOf(root);

            if (node == null)
                throw SceneDoesNotExist;

            nodes[nodes.IndexOf(node)] = nodes[0];
            nodes[0] = node;
        }

        public IScene[] GetAllScenes()
        {
            var result = from node in nodes
                         select node.Scene;
            return result.ToArray();
        }
        public IScene[] GetAllLinkedScenesFor(IScene scene)
        {
            if (scene == null)
                throw new ArgumentNullException();

            var node = GetNodeOf(scene);
            var result = from item in node.Links
                         select item.Value.Scene;
            return result.ToArray();
        }
        public string[] GetAllLinkedTextsFor(IScene scene)
        {
            if (scene == null)
                throw new ArgumentNullException();

            return GetNodeOf(scene)?.GetLinkedTexts();
        }
        public IScene[] GetScenesBy(string text)
        {
            var result = from item in nodes
                         where item.Scene.Text.Contains(text)
                         select item.Scene;

            return result.ToArray();
        }

        public void SetCurrent(IScene scene)
        {
            if (scene == null)
                throw new ArgumentNullException();

            position = GetNodeOf(scene);

            if (position == null)
                throw SceneDoesNotExist;
        }
        public void Remove(IScene scene)
        {
            if (scene == null)
                throw new ArgumentNullException();

            var nodeOfScene = GetNodeOf(scene);

            if (nodeOfScene == null)
                throw SceneDoesNotExist;

            foreach (var node in nodes)
            {
                node.RemoveLink(scene);
            }

            nodes.Remove(nodeOfScene);
        }
        public void RemoveLink(IScene sourceScene, IScene linkedScene)
        {
            if (sourceScene == null || linkedScene == null)
                throw new ArgumentNullException();

            var from = GetNodeOf(sourceScene);
            var to = GetNodeOf(linkedScene);

            if (from == null || to == null)
                throw SceneDoesNotExist;

            from.RemoveLink(to.Scene);
        }
        public void RemoveLink(IScene sourceScene, string linkedText)
        {
            if (sourceScene == null)
                throw new ArgumentNullException();

            var node = GetNodeOf(sourceScene);

            if (node == null)
                throw SceneDoesNotExist;

            node.RemoveLink(linkedText);
        }
    }
}
