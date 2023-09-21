using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TextParser.Comparers;
using TextParser.Extensions;

namespace TextParser
{
    public class Program
    {
        private static readonly char[] separatingSentencesChars = {'.', '!', '?', ';', '\n', '\r'};
        private static readonly char[] separatingWordChars = {' ', ',', ':', '"', '\''};
        private const int CHARS_PER_PREFIX_COUNT = 3;
        private const int MAX_PREFIXES_COUNT = 10;

        private static readonly object locker = new object();

        public static void Main()
        {
            var prefixToCount = new Dictionary<ReadOnlyMemory<char>, int>(new ReadOnlyMemoryComparer());
            var watch = new Stopwatch();
            string? path = Console.ReadLine();

            watch.Start();
            using StreamReader sr = new StreamReader(path);
            foreach (var sentence in sr.ReadLines(separatingSentencesChars))
            {
                var words = sentence.Split(separatingWordChars);
                var allPrefixes = FindAllPrefixes(words, CHARS_PER_PREFIX_COUNT);

                MergeDictionaries(prefixToCount, allPrefixes);
            }
            var mostFrequentPrefixes = GetMostFrequentPrefixes(prefixToCount, MAX_PREFIXES_COUNT);
            watch.Stop();

            foreach ((var key, int value) in mostFrequentPrefixes)
                Console.WriteLine(key.ToString().ToLower() + " " + value);
            Console.WriteLine(watch.ElapsedMilliseconds);
        }

        private static IEnumerable<KeyValuePair<ReadOnlyMemory<char>, int>> GetMostFrequentPrefixes
        (Dictionary<ReadOnlyMemory<char>, int> dict, int max)
        {
            IEnumerable<KeyValuePair<ReadOnlyMemory<char>, int>> mostFrequentPrefixes = dict
                .OrderByDescending(x => x.Value)
                .Take(max);
            return mostFrequentPrefixes;
        }

        private static Dictionary<ReadOnlyMemory<char>, int> FindAllPrefixes(IEnumerable<ReadOnlyMemory<char>> words,
            int prefixCount)
        {
            var result = new Dictionary<ReadOnlyMemory<char>, int>(new ReadOnlyMemoryComparer());

            var partitioner = Partitioner.Create(words);
            Parallel.ForEach(partitioner,
                () => new Dictionary<ReadOnlyMemory<char>, int>(new ReadOnlyMemoryComparer()),
                (word, _, localDict) => FindPrefixesInWord(prefixCount, word, localDict),
                localDict =>
                {
                    lock (locker) MergeDictionaries(result, localDict);
                });
            return result;
        }

        private static void MergeDictionaries(Dictionary<ReadOnlyMemory<char>, int> result,
            params Dictionary<ReadOnlyMemory<char>, int>[] dictionaries)
        {
            foreach (var localDict in dictionaries)
            {
                result.EnsureCapacity(localDict.Count);
                foreach ((var key, int value) in localDict)
                {
                    if (result.ContainsKey(key)) result[key] += value;
                    else result.Add(key, value);
                }
            }
        }

        private static Dictionary<ReadOnlyMemory<char>, int> FindPrefixesInWord(int prefixCount,
            ReadOnlyMemory<char> memory, Dictionary<ReadOnlyMemory<char>, int> localDict)
        {
            for (int i = memory.Length - prefixCount; i >= 0; i--)
            {
                var key = memory.Slice(i, prefixCount);
                if (!localDict.ContainsKey(key)) localDict[key] = 0;
                localDict[key]++;
            }
            return localDict;
        }
    }
}