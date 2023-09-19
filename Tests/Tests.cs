using System;
using System.Collections.Generic;
using NUnit.Framework;
using TextParser.Extensions;

namespace Tests
{
    public class Tests
    {
        [TestFixture]
        public class CorrectSplitting
        {
            [TestCase(",hello,    world,,", new[]{"hello", "    world"}, new char[]{','})]
            [TestCase("     ,     , , , , ,hello,world,,,,", new[]{"hello", "world"}, new char[]{','})]
            [TestCase("b;\tc;\rd;\ne;\r\nf;\r\n\r\ng", new[]{"b","c","d","e","f","g"}, new char[]{'\t', '\r', '\n', ';'})]
            public void EnumeratorSplitting(string input, string[] output, char[] separators)
            {
                var span = input.AsSpan();
                Console.WriteLine(span.ToString());
                var list = new List<string>();
                foreach (var split in span.SplitRemoveEmptyEntries(separators))
                {
                    list.Add(split.ToString());
                }
                Assert.That(list.ToArray(), Is.EqualTo(output));
            }
            [TestCase(",hello,    world,,", new[]{"hello", "    world"}, new char[]{','})]
            [TestCase("     ,     , , , , ,hello,world,,,,", new[]{"hello", "world"}, new char[]{','})]
            [TestCase("b;\tc;\rd;\ne;\r\nf;\r\n\r\ng", new[]{"b","c","d","e","f","g"}, new char[]{'\t', '\r', '\n', ';'})]
            public void TupleSplitting(string input, string[] output, char[] separators)
            {
                var span = input.AsSpan();
                Console.WriteLine(span.ToString());
                var list = new List<string>();
                foreach (var range in span.SplitRemoveEmptyEntriesTuple(separators))
                {
                    var split = span.Slice(range.Item1, range.Item2);
                    list.Add(split.ToString());
                }
                Assert.That(list.ToArray(), Is.EqualTo(output));
            }

            [TestCase(",hello,    world,,", new[]{"hello", "    world"}, new char[]{','})]
            [TestCase("     ,     , , , , ,hello,world,,,,", new[]{"hello", "world"}, new char[]{','})]
            [TestCase("     ,     , , , , ,hello:hi,world,,,,", new[]{"hello","hi", "world"}, new char[]{',', ':'})]
            [TestCase("b;\tc;\rd;\ne;\r\nf;\r\n\r\ng", new[]{"b","c","d","e","f","g"}, new char[]{'\t', '\r', '\n', ';'})]
            [TestCase("“Comb your hair!” he barked.", new[]{"Comb","your","hair","he","barked"}, new char[]{' ', '“', '”', '!', '.'})]
            public void MemoryRangeSplitting(string input, string[] output, char[] separators)
            {
                var memory = input.AsMemory();
                Console.WriteLine(memory.ToString());
                var list = new List<string>();
                foreach (var range in memory.SplitRemoveEmptyEntries(separators))
                {
                    var split = memory.Slice(range.Start.Value, range.End.Value);
                    list.Add(split.ToString());
                }
                Assert.That(list.ToArray(), Is.EqualTo(output));
            }
            [TestCase(",hello,    world,,", new[]{"hello", "    world"}, new char[]{','})]
            [TestCase("     ,     , , , , ,hello,world,,,,", new[]{"hello", "world"}, new char[]{','})]
            [TestCase("     ,     , , , , ,hello:hi,world,,,,", new[]{"hello","hi", "world"}, new char[]{',', ':'})]
            [TestCase("b;\tc;\rd;\ne;\r\nf;\r\n\r\ng", new[]{"b","c","d","e","f","g"}, new char[]{'\t', '\r', '\n', ';'})]
            [TestCase("“Comb your hair!” he barked.", new[]{"Comb","your","hair","he","barked"}, new char[]{' ', '“', '”', '!', '.'})]
            public void MemorySplitting(string input, string[] output, char[] separators)
            {
                var memory = input.AsMemory();
                Console.WriteLine(memory.ToString());
                var list = new List<string>();
                foreach (var split in memory.Split(separators))
                {
                    list.Add(split.ToString());
                }
                Assert.That(list.ToArray(), Is.EqualTo(output));
            }

        }
    }
}