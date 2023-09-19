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
        public IEnumerable<ReadOnlyMemory<char>> ParseToMemory(string path)
        {
      
            var allWords = new List<ReadOnlyMemory<char>>();
            var partitioner = Partitioner.Create(File.ReadLines(path));

            Parallel.ForEach(partitioner,
                () => new List<ReadOnlyMemory<char>>(),
                (line, _, localList) =>
            {
                var memory = line.AsMemory();
                foreach (var word in memory.Split(separatingChars))
                    localList.Add(word);
                return localList;
            }, 
                localList=>
            {
                lock (allWords)
                {
                    foreach (var el in localList)
                    {
                        allWords.Add(el);
                    }
                }
            });
            return allWords;
        }
    }
}