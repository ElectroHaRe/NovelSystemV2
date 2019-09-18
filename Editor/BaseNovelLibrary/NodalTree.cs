using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BaseNovelLibrary
{
    [Serializable]
    public class NodalTree
    {
        public NodalTree() { }
        public NodalTree(INode root) : this()
        {
            branches.Add(new NodalBranch(root));
            CurrentBranch = branches[0];
        }

        public INode this[int index] => GetAllNodes()[index];
        public INode Root => branches.Count > 0 ? branches[0].Node : null;
        public INode CurrentNode => CurrentBranch?.Node;
        public int Count => branches.Count;
        private NodalBranch CurrentBranch;
        private List<NodalBranch> branches = new List<NodalBranch>();

        public INode Next(string text)
        {
            CurrentBranch = CurrentBranch?.Next(text);
            return CurrentNode;
        }
        public INode Previous()
        {
            CurrentBranch = CurrentBranch?.Previous;
            return CurrentNode;
        }
        public void AddNode(INode node)
        {
            foreach (var branch in branches)
            {
                if (branch.Node == node)
                    return;
            }
            branches.Add(new NodalBranch(node));
            if (branches.Count == 1)
                CurrentBranch = branches[0];
        }
        public void AddTie(string text, INode nextNode)
        {
            var temp = branches.Find(item => item.Node == nextNode);
            if (temp == null)
                return;
            CurrentBranch?.AddTie(text, temp);
        }
        public void ChangeRoot(INode node)
        {
            NodalBranch temp = branches.Find(item => item.Node == node);
            if (temp == null)
                temp = new NodalBranch(node);
            else branches.Remove(temp);
            NodalBranch[] array = branches.ToArray();
            branches.Clear();
            branches.Add(temp);
            branches.AddRange(array);
        }
        public INode[] GetAllNodes()
        {
            INode[] nodes = new INode[branches.Count];
            Parallel.For(0, branches.Count, i => nodes[i] = branches[i].Node);
            return nodes;
        }
        public INode[] GetAllNextNodes()
        {
            string[] ties = CurrentBranch.ties;
            INode[] nodes = new INode[ties.Length];
            Parallel.For(0, nodes.Length, i => nodes[i] = CurrentBranch?.Next(ties[i]).Node);
            return nodes;
        }
        public INode[] GetAllNextNodes(INode from)
        {
            INode temp = CurrentNode;
            GoToNode(from);
            var nodes = GetAllNextNodes();
            GoToNode(temp);
            return nodes;
        }
        public string[] GetAllTieTexts()
        {
            return CurrentBranch?.ties;
        }
        public INode[] GetNodesByText(string request)
        {
            INode[] nodes = new INode[0];
            Parallel.ForEach(branches, item =>
            {
                if (Regex.IsMatch(item.Node.Text, "(" + request + ")+"))
                {
                    Array.Resize(ref nodes, nodes.Length + 1);
                    nodes[nodes.Length - 1] = item.Node;
                }
            });
            return nodes;
        }
        public void GoToNode(INode node)
        {
            Parallel.ForEach(branches, item => { if (item.Node == node) { CurrentBranch = item; return; } });
        }
        public void RemoveNode(INode node)
        {
            Parallel.ForEach(branches, item => item.RemoveTie(node));
            for (int i = 0; i < branches.Count; i++)
            {
                if (branches[i].Node == node)
                {
                    branches.Remove(branches[i]);
                    return;
                }
            }
        }
        public void RemoveTie(INode nextNode)
        {
            CurrentBranch?.RemoveTie(nextNode);
        }
        public void RemoveTie(string text)
        {
            CurrentBranch?.RemoveTie(text);
        }
        public void RemoveTie(INode startNode, INode nextNode)
        {
            GoToNode(startNode);
            CurrentBranch?.RemoveTie(nextNode);
        }
        public void RemoveTie(INode startNode, string text)
        {
            GoToNode(startNode);
            CurrentBranch?.RemoveTie(text);
        }
    }
}
