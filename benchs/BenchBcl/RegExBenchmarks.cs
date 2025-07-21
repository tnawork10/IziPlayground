using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using Formulas;
using StackList = GCE.Unsafe.AtStackLinkedList<(string content, int offsetSrcStart, int lengthSrcToCopy, int lengthThisSegment, int offsetAtBuffer)>;

namespace BenchBcl
{
    /* 20250721
| Method              | Mean       | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|-------------------- |-----------:|----------:|----------:|-------:|-------:|----------:|
| OnlyFoundReplace    |   995.8 ns |  48.25 ns | 136.87 ns | 0.1831 | 0.0010 |    1536 B |
| WithRegexAndReplace | 2,466.4 ns | 130.72 ns | 379.25 ns | 0.6828 |      - |    5720 B |
| WithStack           |   595.4 ns |  11.92 ns |  30.12 ns | 0.0448 |      - |     376 B |     
     */
    [MemoryDiagnoser]
    public partial class RegExBenchmarks
    {
        [GeneratedRegex("\\[(.*?)\\]", RegexOptions.Compiled)]
        public static partial Regex CodeRegex();
        /// <summary>
        /// 203 bytes
        /// </summary>
        private const string expr =
            $"[3a242f19-b308-415c-9253-8a3eacd97ac7]+" +
            $"[66a0e635-44d6-4ffe-8f9a-25dede382cd2]+12321312*500+" +
            $"[8430fd33-de43-436b-84cc-ec80accde99e]/2*100+" +
            $"[1bd97b25-a93d-445f-afbc-7ab9d5e001ea] + " +
            $"[6014ed6d-5634-468b-80d1-fc3be6ee0128]";

        private static Guid id0 = Guid.Parse("3a242f19-b308-415c-9253-8a3eacd97ac7");
        private static Guid id2 = Guid.Parse("66a0e635-44d6-4ffe-8f9a-25dede382cd2");
        private static Guid id3 = Guid.Parse("8430fd33-de43-436b-84cc-ec80accde99e");
        private static Guid id4 = Guid.Parse("1bd97b25-a93d-445f-afbc-7ab9d5e001ea");
        private static Guid id5 = Guid.Parse("6014ed6d-5634-468b-80d1-fc3be6ee0128");
        private const string someCode = "SomeCode";

        [Benchmark]
        public void OnlyFoundReplace()
        {
            var replacements = CodeRegex().Matches(expr);
            var expressionGuids = new List<Guid>();

            foreach (var item in replacements.AsEnumerable())
            {
                if (item.Success)
                {

                }
                //var guidStr = item.ToString().Replace("[", "").Replace("]", "").Trim().Replace(" ", "");
            }
        }

        [Benchmark]
        public string WithRegexAndReplace()
        {
            var replacements = CodeRegex().Matches(expr);
            var expressionGuids = new List<Guid>();

            foreach (var item in replacements)
            {
                var guidStr = item.ToString().Replace("[", "").Replace("]", "").Trim().Replace(" ", "");
                expressionGuids.Add(Guid.Parse(guidStr.ToString()));
            }

            var codesDict = new Dictionary<Guid, string>();
            codesDict.Add(id0, someCode);
            codesDict.Add(id2, someCode);
            codesDict.Add(id3, someCode);
            codesDict.Add(id4, someCode);
            codesDict.Add(id5, someCode);

            var newFormulaExpression = expr;
            foreach (var dict in codesDict)
            {
                newFormulaExpression = newFormulaExpression.Replace(dict.Key.ToString(), dict.Value.ToString());
            }
            return newFormulaExpression;
        }

        [Benchmark]
        public void WithStack()
        {
            var packs = GetPacksPerGuid(expr);
            var etor = packs.GetEnumerator();
            var stackListEl = new StackList();
            var result = AllocateString(expr, etor, stackListEl, 0, 0, 0);
        }


        /// <summary>
        /// Рекурсивный метод для замены всех диапазонов с Guid на строку без аллокаций в куче. Стэк растет до последнего элемента, затем выделяется буфер и он заполняется с конца до начала.
        /// </summary>
        public static unsafe string AllocateString(string source,
                                             IEnumerator<(string Content, GuidStringRange Range)> etor,
                                             StackList stackListEl,
                                             int offset,
                                             int lengthBuffer,
                                             int offsetAtSource // индекс любого следующего символа, после последнего ']' с GUID
            )
        {
            // подразумевается что etor идет последовательное через сканирование входной строки (движение только вперед)
            if (etor.MoveNext())
            {
                var pack = etor.Current;
                string content = pack.Content;
                var range = pack.Range;
                int offsetThisRange = offset;
                var contentLength = 0;
                // длина между последним ']' с Guid и следующим(текущем) '[' Guid
                var lengthToCopyFromSource = range.offset - offsetAtSource;
                // шифр + открывающая '[' и закрывающая ']'
                contentLength = content.Length + 2;
                // задаем индекс текущего элемента списка
                var index = stackListEl.index + 1;
                // длина нового сегмента
                var lengthThisSegment = lengthToCopyFromSource + contentLength;
                var offsetNext = offsetThisRange + lengthThisSegment;
                // аккомлируем длину
                var lengthBufferNew = lengthBuffer + lengthThisSegment;
                // следующий за ']' символ - это старт диапазона для вставки
                var offsetAtSourceEnd = range.offset + range.length;
                var stackListElNext = new StackList(&stackListEl, ValueTuple.Create(content, offsetAtSource, lengthToCopyFromSource, lengthThisSegment, offsetThisRange), index);
                return AllocateString(source, etor, stackListElNext, offsetNext, lengthBufferNew, offsetAtSourceEnd);
            }
            else
            {   // если дошли до конца, то создаем буфер и заполняем его обратным ходом (от конца к началу)
                var last = stackListEl;
                var lstSegmentIndex = offsetAtSource;
                var lastSegmentLength = source.Length - lstSegmentIndex;
                // создаем буфер. если стэка не хватить можно арендовать буфер
                Span<char> buffer = stackalloc char[lengthBuffer + lastSegmentLength];
                var index = last.index;
                var currentStackListEl = last;

                while (index >= 0)
                {
                    var val = currentStackListEl.value;
                    var target = buffer.Slice(val.offsetAtBuffer, val.lengthThisSegment);
                    // если перед плейсхолдером в виде [guid] был текст, то копируем его
                    if (val.lengthSrcToCopy > 0)
                    {
                        // сначала копируем диапазон из оригинальной строки
                        var src = source.AsSpan().Slice(val.offsetSrcStart, val.lengthSrcToCopy);
                        var targetToInsertSrc = target.Slice(0, val.lengthSrcToCopy);
#if DEBUG
                        if (src.Length != targetToInsertSrc.Length)
                        {
                            throw new ArgumentOutOfRangeException($"toCopy length: {src.Length}; target length: {target.Length}");
                        }
#endif
                        src.CopyTo(targetToInsertSrc);
                    }
                    var targetPlaceholder = target.Slice(val.lengthSrcToCopy, val.content.Length + 2);
                    targetPlaceholder[0] = '[';
                    var targetForContent = targetPlaceholder.Slice(1, val.content.Length);
                    val.content.AsSpan().CopyTo(targetForContent);
                    targetPlaceholder[targetPlaceholder.Length - 1] = ']';
                    // переходим на предыдущий элемент
                    currentStackListEl = *currentStackListEl.previousPointer;
                    index--;
                }
                if (lastSegmentLength > 0)
                {
                    var lastSegment = source.AsSpan().Slice(lstSegmentIndex, lastSegmentLength);
                    var targetLastSegment = buffer.Slice(offset, lastSegmentLength);
                    lastSegment.CopyTo(targetLastSegment);
                }
                return new string(buffer);
            }
        }

        private IEnumerable<(string Content, GuidStringRange Range)> GetPacksPerGuid(ExpressionWithGuid formulaExpression)
        {
            var ranges = GetGuids(formulaExpression);
            foreach (var range in ranges)
            {
                yield return (someCode, range);
            }
        }
        private IEnumerable<GuidStringRange> GetGuids(ExpressionWithGuid formulaExpression)
        {
            var etor = new FormulaGuidEnumerator(formulaExpression);
            while (etor.MoveNext())
            {
                var guidRange = etor.Current;
                yield return guidRange;
            }
        }
    }
}


