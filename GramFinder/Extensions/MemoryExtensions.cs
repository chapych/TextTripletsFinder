using System;
using System.Collections;
using System.Collections.Generic;

namespace TextParser.Extensions
{
    public static class MemoryExtensions
    {
        public static Tuple<int,int>[] SplitRemoveEmptyEntriesTuple(this ReadOnlyMemory<char> initialMemory, char[] separators)
        {
            var result = new List<Tuple<int, int>>();
            int max = initialMemory.Length;
            int pos = 0;
            while(true)
            {
                var memory = initialMemory;
                if (memory.Length == 0)
                    break;

                int index = memory.Span.IndexOfAny(separators);
                if (index == -1)
                {
                    result.Add(new Tuple<int, int>(pos, max-pos));
                    break;
                }
                initialMemory = memory.Slice(index + 1);
                var current = memory.Slice(0, index);
                if(index!=0 && !current.Span.IsWhiteSpace())
                    result.Add(new Tuple<int, int>(pos, index));
                pos = pos + index + 1;
            }
            return result.ToArray();
        }
    }
}