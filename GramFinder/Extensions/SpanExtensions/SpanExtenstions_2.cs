﻿using System;
 using System.Collections;
 using System.Collections.Generic;

namespace TextParser.Extensions
{
    public static partial class SpanExtensions
    {
        public static Tuple<int,int>[] SplitRemoveEmptyEntriesTuple(this ReadOnlySpan<char> initialSpan, char[] separators)
        {
            var result = new List<Tuple<int, int>>();
            int max = initialSpan.Length;
            int pos = 0;
            while(true)
            {
                var span = initialSpan;
                if (span.Length == 0)
                    break;

                int index = span.IndexOfAny(separators);
                if (index == -1)
                {
                    result.Add(new Tuple<int, int>(pos, max-pos));
                    break;
                }
                initialSpan = span.Slice(index + 1);
                var current = span.Slice(0, index);
                if(index!=0 && !current.IsWhiteSpace())
                    result.Add(new Tuple<int, int>(pos, index));
                pos = pos + index + 1;
            }
            return result.ToArray();
        }
    }
}