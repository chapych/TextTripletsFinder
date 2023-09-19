using System;
using System.Collections.Generic;

namespace TextParser.Extensions
{
    public static partial class SpanExtensions
    {
        public static SpanSplitEnumerator SplitRemoveEmptyEntries(this ReadOnlySpan<char> span, char[] separators)
        {
            return new SpanSplitEnumerator(span, separators);
        }
    }
    public ref struct SpanSplitEnumerator
    {
        private ReadOnlySpan<char> initialSpan;
        private readonly char[] separators;

        public SpanSplitEnumerator(ReadOnlySpan<char> initialSpan, char[] separators)
        {

            this.initialSpan = initialSpan;
            this.separators = separators;
            Current = default;
        }
        public SpanSplitEnumerator GetEnumerator() { return this;  }
        public bool MoveNext()
        {
            while(true)
            {
                var span = initialSpan;
                if (span.Length == 0)
                    return false;

                int index = span.IndexOfAny(separators);
                if (index == -1)
                {
                    initialSpan = ReadOnlySpan<char>.Empty;
                    Current = span;
                    return true;
                }
                initialSpan = span.Slice(index + 1);
                Current = span.Slice(0, index);
                if(index==0 || Current.IsWhiteSpace()) continue;
                return true;
            }
        }

        public ReadOnlySpan<char> Current { get; private set; }
    }
}