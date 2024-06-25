namespace Unsafe
{
    public unsafe class StackListTests
    {
        private class Someclass
        {
            public int val = 5012;
        }
        public static void Do()
        {
            var vaa = new Someclass();
            var list = new StackList<Someclass>(&vaa, null);
            ref var refVar = ref list.ValueByRef;

            Console.WriteLine(refVar.val);
    


        }
    }
    public unsafe ref struct StackList<T>
    {
        private delegate bool Iterate(ref IEnumerator<T> etor);

        private readonly int index;
        private StackList<T>* previousElement;
        private T* pointerToValue;
        public ref T ValueByRef => ref *pointerToValue;
        public T Value => *pointerToValue;

        public StackList(T* item, StackList<T>* previousElement)
        {
            pointerToValue = item;
            this.previousElement = previousElement;
            if (previousElement != null)
            {
                index = previousElement->index + 1;
            }
        }

        private void SetPointerToPrevious(StackList<T>* p)
        {
            previousElement = p;
        }

        public bool Contains(T value)
        {
            if (value.Equals(*pointerToValue)) return true;

            for (int i = index; i >= 0; i--)
            {
                var previus = *previousElement;
                if (previus.CompareWithValue(value)) return true;
            }
            return false;
        }

        public bool CompareWithValue(T value) => value.Equals(*pointerToValue);


        public static void Workaround()
        {
            var type = typeof(StackList<T>);
            var head = new StackList<Type>();
            head.SetPointerToPrevious(&head);

            RecursiveV3(head, type);
            RecursiveV2(head, type);
            Recursive(head, type);
        }

        /// <summary>
        /// Каждый 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="action"></param>
        public void Add(StackList<T>* previous, T item, Func<T, T?> action)
        {
            T? nextItem = action(item);
            if (nextItem != null)
            {
                var element = new StackList<T>(&nextItem, previous);
                Add(&element, nextItem, action);
            }
        }
        public void Add(StackList<T>* previous, T[] values, int index, Func<T, bool> predictate)
        {
            if (values.Length > index)
            {
                T item = values[index];
                var element = new StackList<T>(&item, previous);
                if (predictate(item))
                {
                    Add(&element, values, index + 1, predictate);
                }
                //values.GetEnumerator<T>();
                throw new System.NotImplementedException();
            }
        }
        public void Add(in IEnumerator<T> etor, StackList<T> previous, Func<T, bool> predictate)
        {
            if (etor.MoveNext())
            {
                T next = etor.Current;
                var element = new StackList<T>(&next, &previous);
                if (predictate(next))
                {
                    Add(in etor, element, predictate);
                }
            }
        }

        public StackList<T>* GoTo(int index)
        {
            if (index > this.index) throw new ArgumentOutOfRangeException($"Current index is: {this.index}. recieved: {index.ToString()}");
            var pointer = this.previousElement;
            for (int i = 0; i < index; i++)
            {
                pointer = pointer->previousElement;
            }
            return pointer;
        }

        /// <summary>
        /// Begin list 
        /// </summary>
        /// <param name="value">Must be in stack</param>
        /// <returns></returns>
        public static StackList<T> Begin(T* value) => new StackList<T>(value, null);

        private static void RecursiveV3(StackList<Type> head, Type type)
        {
            head.Add(&head, type, (x) =>
            {
                return null;
            });
            throw new System.NotImplementedException();
        }


        private static void RecursiveV2(StackList<Type> head, Type type)
        {
            var types = type.Assembly.GetTypes();
            //StackList<Type>.Foreach(types.Length, head);
        }
        private static void Recursive(StackList<Type> head, Type type)
        {
            var types = type.Assembly.GetTypes();
            Console.WriteLine(type.AssemblyQualifiedName);
            StackList<Type>* span = stackalloc StackList<Type>[types.Length];
            var links = stackalloc Type*[10];
            //Span<Type> spanOfTypes = new Span<Type>(links);
            //IntPtr

            foreach (var item in types)
            {
                if (!head.Contains(item))
                {
                    if (item.BaseType != null)
                    {
                        Recursive(default, item.BaseType);
                        throw new System.NotImplementedException();
                    }
                }
            }
        }

        private static void Foreach(int index, T[] values, StackList<T> list)
        {
            throw new System.NotImplementedException();
        }
        private static void Chain(ref IEnumerator<T> enumerator, StackList<T> list)
        {
            throw new System.NotImplementedException();
        }

    }
}
