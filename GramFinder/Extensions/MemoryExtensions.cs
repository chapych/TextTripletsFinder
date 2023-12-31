﻿using System;
using System.Collections.Generic;

namespace TextParser.Extensions
{
    public static class MemoryExtensions
    {
        public static IEnumerable<ReadOnlyMemory<char>> Split(this ReadOnlyMemory<char> initialMemory,
            params char[] separators)
        {
            int pos = 0;
            while(true)
            {
                var memory = initialMemory;
                if (memory.Length == 0)
                    break;

                int index = memory.Span.IndexOfAny(separators);
                if (index == -1)
                {
                    yield return memory[..];
                    break;
                }
                initialMemory = memory[(index + 1)..];
                var current = memory[..index];
                if (index != 0 && !current.Span.IsWhiteSpace())
                    yield return current;
                pos = pos + index + 1;
            }
        }
    }
}