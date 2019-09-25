using System;
using System.Linq;
using System.Collections.Generic;

namespace Library
{
    [Serializable]
    internal class Node
    {
        internal Node(IScene value)
        {
            Scene = value;
        }

        internal IScene Scene { get; private set; }
        internal Dictionary<string, Node> Links { get; private set; } = new Dictionary<string, Node>();

        internal bool AddLink(string text, Node node)
        {
            if (text == null && node == null)
                throw new ArgumentNullException("text & node");
            else if (text == null)
                throw new ArgumentNullException("text");
            else if (node == null)
                throw new ArgumentNullException("node");

            if (Links.ContainsKey(text))
                return false;

            Links.Add(text, node);
            return true;
        }
        internal bool RemoveLink(IScene node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            var result = from pair in Links
                         where pair.Value == node
                         select pair.Key;

            var keys = result;

            if (keys.Count() == 0)
                return false;

            foreach (var key in keys)
            {
                Links.Remove(key);
            }

            return true;
        }
        internal bool RemoveLink(string text)
        {
            if (text == null)
                throw new ArgumentNullException();

            return Links.Remove(text);
        }
        internal void RemoveAllLinks()
        {
            Links = new Dictionary<string, Node>();
        }
        internal string[] GetLinkedTexts()
        {
            string[] texts = new string[Links.Count];
            Links.Keys.CopyTo(texts, 0);
            return texts;
        }
    }
}
