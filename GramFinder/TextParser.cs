using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TextParser.Extensions;

namespace TextParser
{
    public sealed class TextParser
    {
        private readonly char[] separatingChars;

        public TextParser(char[] separatingChars) => this.separatingChars = separatingChars;
        
        public List<ReadOnlyMemory<char>> ParseToMemory(string path)
        {
            var allWords = new List<ReadOnlyMemory<char>>();
            try
            {
                var partitioner = Partitioner.Create(File.ReadLines(path));

                Parallel.ForEach(partitioner,
                    () => new List<ReadOnlyMemory<char>>(),
                    (line, _, localList) =>
                    {
                        var memory = line.AsMemory();
                        localList.AddRange(memory.Split(separatingChars));
                        return localList;
                    },
                    localList =>
                    {
                        lock (allWords)
                        {
                            allWords.EnsureCapacity(localList.Count);
                            allWords.AddRange(localList);
                        }
                    });
            }
            catch (Exception ex)
            {
                if (ex is AggregateException || ex is OutOfMemoryException)
                {
                    Console.WriteLine("Large Size File");
                    allWords.Clear();
                    using StreamReader sr = new StreamReader(path, Encoding.Default);
                    foreach (var memory in sr.ReadLines(separatingChars))
                        allWords.AddRange(memory.Split(separatingChars));
                }
            }
            return allWords;
        }
    }
}