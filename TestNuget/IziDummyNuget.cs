using Stroyplatforma.TestLibs.ValueGenerators;

namespace IziHardGames.TestNuget
{
    public class IziDummyNuget
    {
        public long value0;
        public long value1;
        public long value2;
    }

    public class Test
    {
        static void Main(string[] args)
        {
            var ar = new int[10];
            var type = ar.GetType();
            Console.WriteLine($"{type.FullName}; isgen: {type.IsGenericType}");
        }


        internal class Root
        {
            public Nested1? Nested1AsProp { get; set; }
            public Nested1? Nested1AsField;
            public List<Nested1?> nested1AsCollection;
            public Nested2 nested2;
            public Nested3 nested3;
        }

        internal class Nested1
        {
            public string? s0;
            public float f0;
            public float? fNull0;
        }

        internal struct Nested2
        {
        }

        internal struct Nested3
        {
            private Nested1? head;
            private Nested1? tail;
        }
    }
}