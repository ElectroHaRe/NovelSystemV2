namespace WFControlLibrary
{
    class CircularStack<T>
    {
        public CircularStack(int capacity) { items = new T[capacity]; }

        private int Capacity => items.Length;
        public int Count { get; private set; } = 0;

        public bool isEmpty => Count == 0;
        public bool isFull => Count == Capacity;

        private int head = 0;
        private int tail = 0;

        private T[] items;

        public CircularStack<T> Push(T item)
        {
            items[tail] = item;
            tail = (tail + 1) % Capacity;
            if (head == tail)
                head = (head + 1) % Capacity;
            Count++;
            return this;
        }
        public T Peek()
        {
            return items[(tail - 1 + Capacity) % Capacity];
        }
        public T Pop()
        {
            if (head == tail)
                return items[tail];
            tail = (tail - 1 + Capacity) % Capacity;
            var temp = items[tail];
            if (head == tail)
                this.Clear();
            return temp;
        }

        public void Clear()
        {
            head = tail = Count = 0;
            items = new T[Capacity];
        }
    }
}
