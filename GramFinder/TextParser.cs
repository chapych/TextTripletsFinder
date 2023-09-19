using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TextParser.Extensions;

namespace TextParser
{
    public class TextParser
    {
        private readonly char[] separatingChars;

        public TextParser(char[] separatingChars) => this.separatingChars = separatingChars;

        public IEnumerable<string> Parse(string path)
        {
            var allWords = new ConcurrentBag<string>();
            try
            {
                var partitioner = Partitioner.Create(File.ReadLines(path));
                Parallel.ForEach(partitioner, line =>
                {
                    string[] sentences = line.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);
                        allWords.AddRange(sentences);
                });
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Empty String");
            }


            return allWords;
        }
    }
}