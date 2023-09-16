using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;

namespace TextParser
{
    public class Program
    {
        private const int CHARS_PER_PREFIX_COUNT = 3;
        private const int MAX_PREFIXES_COUNT = 10;

        public static void Main()
        {
            char[] separatingChars = {'.', '!', '?', ';', ':', '(', ')',' ', ',', '"'};

            var textParser = new TextParser(separatingChars);
            var prefixFinder = new PrefixFinder();
            var watch = new Stopwatch();
            string? path = Console.ReadLine();

            watch.Start();
            var words = textParser.Parse(path);

            var combinations = prefixFinder.GetMostFrequentPrefixesToCount
            (
                words,
                CHARS_PER_PREFIX_COUNT,
                MAX_PREFIXES_COUNT
            );
            watch.Stop();

            foreach (var keyValuePair in combinations)
                Console.WriteLine(keyValuePair.Key + " " + keyValuePair.Value);
            Console.WriteLine(watch.ElapsedMilliseconds);
        }
    }
}