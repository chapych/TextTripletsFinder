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
            var allPrefixes = FindAllDoubleGrams(text, prefixCount);
            var mostFrequentPrefixes = allPrefixes
                .OrderByDescending(x => x.Value)
                .Take(max);

            return mostFrequentPrefixes;
        }
        private ConcurrentDictionary<string, int> FindAllDoubleGrams(IEnumerable<string> words, int prefixCount)
        {
            var prefixToCount = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            Parallel.ForEach(words, word =>
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
    }
}