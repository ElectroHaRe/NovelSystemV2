using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BaseNovelLibrary
{
    [Serializable]
    internal class NodalBranch 
    {
        internal NodalBranch(INode value)
        {
            Node = value;
        }

        private Dictionary<string, NodalBranch> branches = new Dictionary<string, NodalBranch>();
        internal string[] ties
        {
            get
            {
                string[] temp = new string[0];
                foreach (var item in branches)
                {
                    Array.Resize(ref temp, temp.Length + 1);
                    temp[temp.Length - 1] = item.Key;
                }
                return temp;
            }
        }
        internal NodalBranch Previous { get; private set; }
        internal INode Node { get; private set; }

        internal NodalBranch Next(string text)
        {
            try
            {
                var temp = branches[text];
                temp.Previous = this;
                return temp;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
        internal void RemoveTie(INode node)
        {
            string[] keys = new string[0];
            Parallel.ForEach(branches, item => { if (item.Value == node) { Array.Resize(ref keys, keys.Length + 1); keys[keys.Length - 1] = item.Key; }; });
            Parallel.ForEach(keys, key => branches.Remove(key));
        }
        internal void RemoveTie(string text)
        {
            branches.Remove(text);
        }
        internal void AddTie(string text, NodalBranch nodalBranch)
        {
            branches.Add(text, nodalBranch);
        }
    }
}
