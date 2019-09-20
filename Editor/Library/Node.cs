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
        internal Node Previous { get; private set; }

        internal Node Next(string text)
        {
            try
            {
                var temp = Links[text];
                temp.Previous = this;
                return temp;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
        internal void RemoveLink(IScene node)
        {
            var keys = from pair in Links
                       where pair.Value == node
                       select pair.Key;

            foreach (var key in keys)
            {
                Links.Remove(key);
            }
        }
        internal void RemoveLink(string text)
        {
            Links.Remove(text);
        }
        internal void AddLink(string text, Node nodalBranch)
        {
            Links.Add(text, nodalBranch);
        }
        internal string[] GetLinkedTexts()
        {
            string[] temp = new string[Links.Count];
            Links.Keys.CopyTo(temp, 0);
            return temp;
        }
    }
}
