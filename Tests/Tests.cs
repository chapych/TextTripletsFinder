using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            [TestCase("     ,     , , , , ,hello:hi,world,,,,", new[]{"hello","hi", "world"}, new char[]{',', ':'})]
            [TestCase("b;\tc;\rd;\ne;\r\nf;\r\n\r\ng", new[]{"b","c","d","e","f","g"}, new char[]{'\t', '\r', '\n', ';'})]
            [TestCase("“Comb your hair!” he barked.", new[]{"Comb","your","hair","he","barked"}, new char[]{' ', '“', '”', '!', '.'})]
            public void MemorySplitting(string input, string[] output, char[] separators)
            {
                var memory = input.AsMemory();
                Console.WriteLine(memory.ToString());
                Assert.That(memory.Split(separators).Select(split => split.ToString()).ToArray(), Is.EqualTo(output));
            }
        }

        public class CorrectReading
        {
            [TestCase("test.txt", new[] {"hello", "world"}, new[] {'!'})]
            public void EnumeratorSplitting(string input, string[] output, char[] separators)
            {
                using StreamReader sr = new StreamReader(input, Encoding.Default);

                Assert.That(sr.ReadLines(separators).Select(line => line.ToString()).ToArray(), Is.EqualTo(output));
            }
        }
    }
}