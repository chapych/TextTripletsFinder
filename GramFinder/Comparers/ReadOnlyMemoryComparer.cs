using System;
using System.Collections.Generic;

namespace TextParser.Comparers
{
    public class ReadOnlyMemoryComparer : EqualityComparer<ReadOnlyMemory<char>>
    {
        public override bool Equals(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y)
        {
            return x.Span.Equals(y.Span, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode(ReadOnlyMemory<char> obj)
        {
            return string.GetHashCode(obj.Span, StringComparison.OrdinalIgnoreCase);
        }
    }
}