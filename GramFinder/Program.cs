using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TextParser
{
    public class Program
    {
        private const int CHARS_PER_PREFIX_COUNT = 3;
        private const int MAX_PREFIXES_COUNT = 10;
        public static void Main()
        {
            char[] separatingChars = { '.', '!', '?', ';', ':', '(', ')', ' ', ',', '"' };

            var textParser = new TextParser(separatingChars);
            var prefixFinder = new PrefixFinder();
            var watch = new Stopwatch();
            string? path = Console.ReadLine();

            watch.Start();

            var combination = textParser.ParseToMemory(path);
            var dict = prefixFinder.GetMostFrequentPrefixesToCount(combination, CHARS_PER_PREFIX_COUNT, MAX_PREFIXES_COUNT);

            watch.Stop();

            foreach ((var key, int value) in dict)
                Console.WriteLine(key.ToString().ToLower() + " " + value);
            Console.WriteLine(watch.ElapsedMilliseconds);
        }
    }
}