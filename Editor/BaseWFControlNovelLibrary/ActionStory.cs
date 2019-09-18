using System;
using System.Collections.Generic;
namespace BaseWFControlNovelLibrary
{
    class ActionStory
    {
        public class Command<T>
        {
            public Command(string name, Action<T> undo, Action<T> redo) { this.Name = name; this.Undo = undo; this.Redo = redo; }
            public readonly string Name;
            public readonly Action<T> Undo;
            public readonly Action<T> Redo;

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
                if (!(obj is Command<T>))
                    return false;
                return this.ToString() == obj.ToString();
            }
        }

        public ActionStory(int capacity)
        {
            CommandList = new Dictionary<string, Command<object[]>>(capacity);
            story = new CircularStack<KeyValuePair<string, object[]>>(capacity);
            backupStory = new CircularStack<KeyValuePair<string, object[]>>(capacity);
        }

        private Dictionary<string, Command<object[]>> CommandList;
        private CircularStack<KeyValuePair<string, object[]>> story;
        private CircularStack<KeyValuePair<string, object[]>> backupStory;

        public ActionStory AddCommand(Command<object[]> command)
        {
            CommandList.Add(command.Name, command);
            return this;
        }
        public ActionStory AddCommand(string name, Action<object[]> undo, Action<object[]> redo)
        {
            var temp = new Command<object[]>(name, undo, redo);
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
            Command<object[]> temp;
            var story_item = story.Pop();
            if (CommandList.TryGetValue(story_item.Key, out temp))
            {
                temp.Undo.Invoke(story_item.Value);
                backupStory.Push(story_item);
            }
        }
        public void Redo()
        {
            if (backupStory.Count == 0)
                return;
            Command<object[]> temp;
            var story_item = backupStory.Pop();
            if (CommandList.TryGetValue(story_item.Key, out temp))
            {
                temp.Redo.Invoke(story_item.Value);
                story.Push(story_item);
            }
        }
    }
}
