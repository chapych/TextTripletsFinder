using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TextParser.Comparers;

namespace TextParser
{
    public sealed class PrefixFinder
    {
        public IEnumerable<KeyValuePair<ReadOnlyMemory<char>, int>> GetMostFrequentPrefixesToCount(List<ReadOnlyMemory<char>> words,
            int prefixCount, int max)
        {
            var allPrefixes = FindAllPrefixesInMemory(words, prefixCount);
            IEnumerable<KeyValuePair<ReadOnlyMemory<char>, int>> mostFrequentPrefixes = allPrefixes
                .OrderByDescending(x => x.Value)
                .Take(max);

            return mostFrequentPrefixes;
        }
        
        private Dictionary<ReadOnlyMemory<char>, int> FindAllPrefixesInMemory(IReadOnlyCollection<ReadOnlyMemory<char>> words, int prefixCount)
        {
            var result = new Dictionary<ReadOnlyMemory<char>, int>(prefixCount * words.Count, new ReadOnlyMemoryComparer());

            var partitioner = Partitioner.Create(words);
            Parallel.ForEach(partitioner,
            () => new Dictionary<ReadOnlyMemory<char>, int>(new ReadOnlyMemoryComparer()),
            (memory, _, localDict) =>
            {
                for (int i = memory.Length - prefixCount; i >= 0; i--)
                {
                    var key = memory.Slice(i, prefixCount);
                    if (!localDict.ContainsKey(key)) localDict[key] = 0;
                    localDict[key]++;
                }
                return localDict;
            },
            localDict =>
            {
                lock (result) 
                { 
                    result.EnsureCapacity(localDict.Count);
                    foreach ((var key, int value) in localDict)
                    {
                        if (result.ContainsKey(key)) result[key] += value;
                        else result.Add(key, value);
                    }
                }
            });
            return result;
        }
    }
}
