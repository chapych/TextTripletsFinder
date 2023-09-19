using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TextParser.Comparers;
using TextParser.Extensions;

namespace TextParser
{
    public class PrefixFinder_new
    {
        public IEnumerable<KeyValuePair<ReadOnlyMemory<char>, int>> Find(string path, char[] separatingChars)
        {
            var prefixToCount = new ConcurrentDictionary<ReadOnlyMemory<char>, int>(new ReadOnlyMemoryComparer());
            var prefixFinder = new PrefixFinder();
            var initial = File.ReadLines(path);

            foreach (string line in initial)
            {
                var memory = line.AsMemory();
                //  Parallel.ForEach(memory.SplitRemoveEmptyEntriesTuple(separatingChars), range =>
                //      {
                //          prefixFinder.FindAllPrefixes(memory.Slice(range.Item1, range.Item2), prefixToCount, 3);
                //      }
                // );
                foreach (var range in memory.SplitRemoveEmptyEntriesTuple(separatingChars))
                {
                    prefixFinder.FindAllPrefixes(memory.Slice(range.Item1, range.Item2), prefixToCount, 3);
                }
            }
            Console.WriteLine(prefixToCount.TryGetValue("Wha".AsMemory(), out int _));
            //Console.WriteLine(prefixToCount.Count());
            return prefixToCount
                .OrderByDescending(x => x.Value)
                .Take(10);
        }
    }
}