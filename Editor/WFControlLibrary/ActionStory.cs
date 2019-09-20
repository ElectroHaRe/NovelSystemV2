using System;
using System.Collections.Generic;
namespace WFControlLibrary
{
    class ActionStory
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

        public ActionStory(int capacity)
        {
            CommandList = new Dictionary<string, Command>(capacity);
            story = new CircularStack<KeyValuePair<string, object[]>>(capacity);
            backupStory = new CircularStack<KeyValuePair<string, object[]>>(capacity);
        }

        private Dictionary<string, Command> CommandList;
        private CircularStack<KeyValuePair<string, object[]>> story;
        private CircularStack<KeyValuePair<string, object[]>> backupStory;

        public ActionStory AddCommand(Command command)
        {
            CommandList.Add(command.Name, command);
            return this;
        }
        public ActionStory AddCommand(string name, Action<object[]> undo, Action<object[]> redo)
        {
            var temp = new Command(name, undo, redo);
            CommandList.Add(temp.Name, temp);
            return this;
        }

        public ActionStory Store(string commandName,params object[] args)
        {
            if (!backupStory.isEmpty)
                backupStory.Clear();
            story.Push(new KeyValuePair<string, object[]>(commandName, args));
            return this;
        }

        public void Undo()
        {
            if (story.Count == 0)
                return;
            var storyItem = story.Pop();
            CommandList[storyItem.Key].Undo.Invoke(storyItem.Value);
            backupStory.Push(storyItem);
        }
        public void Redo()
        {
            if (backupStory.Count == 0)
                return;
            var storyItem = backupStory.Pop();
            CommandList[storyItem.Key].Redo.Invoke(storyItem.Value);
            story.Push(storyItem);
        }
    }
}
