using System;
using Library;
using System.Collections.Generic;

namespace WFControlLibrary
{
    class CommandHistory
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

        public CommandHistory(int capacity)
        {
            CommandList = new Dictionary<string, Command>(capacity);
            history = new CircularStack<KeyValuePair<string, object[]>>(capacity);
            backup = new CircularStack<KeyValuePair<string, object[]>>(capacity);
        }

        private Dictionary<string, Command> CommandList;
        private CircularStack<KeyValuePair<string, object[]>> history;
        private CircularStack<KeyValuePair<string, object[]>> backup;

        public CommandHistory AddCommand(Command command)
        {
            CommandList.Add(command.Name, command);
            return this;
        }
        public CommandHistory AddCommand(string name, Action<object[]> undo, Action<object[]> redo)
        {
            var temp = new Command(name, undo, redo);
            CommandList.Add(temp.Name, temp);
            return this;
        }

        public CommandHistory Store(string commandName, params object[] args)
        {
            if (backup.Count > 0)
                backup.Clear();

            history.Push(new KeyValuePair<string, object[]>(commandName, args));
            return this;
        }

        public void Undo()
        {
            if (history.Count == 0)
                return;

            var storyItem = history.Pop();
            CommandList[storyItem.Key].Undo(storyItem.Value);
            backup.Push(storyItem);
        }
        public void Redo()
        {
            if (backup.Count == 0)
                return;

            var storyItem = backup.Pop();
            CommandList[storyItem.Key].Redo(storyItem.Value);
            history.Push(storyItem);
        }
    }
}
