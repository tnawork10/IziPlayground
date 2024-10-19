using BenchmarkDotNet.Attributes;

namespace BenchBcl
{
    public class StringConcatenationBenchmarks
    {
        private const string str1 = "Hello";
        private const string str2 = "World";

        [Benchmark]
        public string StringConcat() => string.Concat(str1, str2);

        [Benchmark]
        public string StringInterpolation() => $"{str1} {str2}";

        [Benchmark]
        public string StringBuilderConcat()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(str1);
            sb.Append(str2);
            return sb.ToString();
        }
    }
}
