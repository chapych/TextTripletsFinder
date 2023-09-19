using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TextParser
{
    public class PrefixFinder
    {
        public IEnumerable<KeyValuePair<string, int>> GetMostFrequentPrefixesToCount(IEnumerable<string> text,
            int prefixCount, int max)
        {
            var allPrefixes = FindAllPrefixes(text, prefixCount);
            var mostFrequentPrefixes = allPrefixes
                .OrderByDescending(x => x.Value)
                .Take(max);

            return mostFrequentPrefixes;
        }
        private ConcurrentDictionary<string, int> FindAllPrefixes(IEnumerable<string> words, int prefixCount)
        {
            var prefixToCount = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var partitioner = Partitioner.Create(words);
             Parallel.ForEach(partitioner, word =>
             {
                 if (word.Length < prefixCount) return;
                 for (int i = word.Length - prefixCount; i >= 0; i--)
                 {
                     string key = word.Substring(i, prefixCount);
                     prefixToCount.AddOrUpdate(key, addValue: 1, updateValueFactory: (_, value) => ++value);
                 }
             });
             return prefixToCount;
        }

        public void FindAllPrefixes(ReadOnlyMemory<char> word, ConcurrentDictionary<ReadOnlyMemory<char>, int> prefixToCount,
            int prefixCount)
        {
            if (word.Length < prefixCount) return;
            for (int i = word.Length - prefixCount; i >= 0; i--)
            {
                var key = word.Slice(i, prefixCount);
                prefixToCount.AddOrUpdate(key, addValue: 1, updateValueFactory: (_, value) => ++value);

                if (word.ToString() == "the")
                {
                    Console.WriteLine("the".AsMemory().Equals(key));
                   // Console.WriteLine("the".AsMemory().Span.ToString() + " " + key.Span.ToString());
                    //prefixToCount.TryGetValue("the".AsMemory(), out int index);
                    //Console.WriteLine("word " + word + " " + " key:" + key + " " + index);
                }
            }
        }
    }
}