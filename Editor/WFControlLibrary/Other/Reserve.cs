using System;
using Library;
using System.Collections.Generic;

namespace WFControlLibrary
{
    class Reserve
    {
        public class Command
        {
            public Command(string name, Action<object[]> undo, Action<object[]> redo)
            {
                this.Name = name;
                this.Undo = undo;
                this.Redo = redo;
            }
            public readonly string Name;
            public readonly Action<object[]> Undo;
            public readonly Action<object[]> Redo;

            public override string ToString()
            {
                return this.Name;
            }
            public override int GetHashCode()
            {
                return this.Name.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if (!(obj is Command))
                    return false;
                return this.ToString() == obj.ToString();
            }
        }

        public Reserve(int capacity)
        {
            CommandList = new Dictionary<string, Command>(capacity);
            story = new CircularStack<KeyValuePair<string, object[]>>(capacity);
            backup = new CircularStack<KeyValuePair<string, object[]>>(capacity);
        }

        private Dictionary<string, Command> CommandList;
        private CircularStack<KeyValuePair<string, object[]>> story;
        private CircularStack<KeyValuePair<string, object[]>> backup;

        public Reserve AddCommand(Command command)
        {
            CommandList.Add(command.Name, command);
            return this;
        }
        public Reserve AddCommand(string name, Action<object[]> undo, Action<object[]> redo)
        {
            var temp = new Command(name, undo, redo);
            CommandList.Add(temp.Name, temp);
            return this;
        }

        public Reserve Store(string commandName, params object[] args)
        {
            if (backup.Count > 0)
                backup.Clear();

            story.Push(new KeyValuePair<string, object[]>(commandName, args));
            return this;
        }

        public void Undo()
        {
            if (story.Count == 0)
                return;

            var storyItem = story.Pop();
            CommandList[storyItem.Key].Undo(storyItem.Value);
            backup.Push(storyItem);
        }
        public void Redo()
        {
            if (backup.Count == 0)
                return;

            var storyItem = backup.Pop();
            CommandList[storyItem.Key].Undo(storyItem.Value);
            story.Push(storyItem);
        }
    }
}
