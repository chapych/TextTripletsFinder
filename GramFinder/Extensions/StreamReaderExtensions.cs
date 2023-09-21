using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextParser.Extensions
{
    public static class StreamReaderExtensions
    {
        public static IEnumerable<ReadOnlyMemory<char>> ReadLines (this TextReader reader,
            params char[] separators)
        {
            List<char> chars = new List<char> ();
            while (reader.Peek() >= 0)
            {
                char c = (char)reader.Read ();
                if (separators.Contains(c))
                {
                    yield return new ReadOnlyMemory<char>(chars.ToArray());
                    chars.Clear();
                    continue;
                }
                chars.Add(c);
            }
        }
    }
}