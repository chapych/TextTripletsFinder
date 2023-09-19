using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TextParser.Comparers;

namespace TextParser
{
    public class PrefixFinder
    {
        public IEnumerable<KeyValuePair<ReadOnlyMemory<char>, int>> GetMostFrequentPrefixesToCount(IEnumerable<ReadOnlyMemory<char>> text,
            int prefixCount, int max)
        {
            var allPrefixes = FindAllPrefixesInMemory(text, prefixCount);
            IEnumerable<KeyValuePair<ReadOnlyMemory<char>, int>> mostFrequentPrefixes = allPrefixes
                .OrderByDescending(x => x.Value)
                .Take(max);

            return mostFrequentPrefixes;
        }
        
        private Dictionary<ReadOnlyMemory<char>, int> FindAllPrefixesInMemory(IEnumerable<ReadOnlyMemory<char>> words, int prefixCount)
        {
            var result = new Dictionary<ReadOnlyMemory<char>, int>(new ReadOnlyMemoryComparer());

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
                   foreach(var kvp in localDict)
                    {
                        if (result.ContainsKey(kvp.Key)) result[kvp.Key] += kvp.Value;
                        else result.Add(kvp.Key, kvp.Value);
                    }
                }
            });
            return result;
        }
    }
}