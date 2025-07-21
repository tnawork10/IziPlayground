namespace GCE.Unsafe
{
    public unsafe ref struct AtStackLinkedList<T>
    {
        public AtStackLinkedList<T>* previousPointer;
        public T value;
        public int index;
        public T this[int index] { get => GetValueAt(index); }

        public AtStackLinkedList()
        {
            previousPointer = default;
            value = default;
            index = -1;
        }
        public AtStackLinkedList(AtStackLinkedList<T>* previous, T value, int index)
        {
            this.previousPointer = previous;
            this.value = value;
            this.index = index;
        }

        public T GetValueAt(int index)
        {
            // можно переходить только левее текущего индекса
            if (index > this.index || index < 0)
            {
                throw new ArgumentOutOfRangeException("Индекс должен быть меньше или равен текущему элементу");
            }
            var pointer = this.previousPointer;
            for (int i = this.index; i != index; i--)
            {
                var prevLinkedListEl = *pointer;
                pointer = prevLinkedListEl.previousPointer;
            }
            var targetLinkedListEl = *pointer;
            return targetLinkedListEl.value;
        }

        

    }
}
