using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using TextParser.Comparers;

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
            string path = "D:/c#/TextAnalysis.csproj/HarryPotterText.txt";

            watch.Start();
            var combination = textParser.ParseToMemory(path);
            var dict = prefixFinder.GetMostFrequentPrefixesToCount(combination, CHARS_PER_PREFIX_COUNT, MAX_PREFIXES_COUNT);
            //var comb = textParser.Parse(path);
            //var dict = prefixFinder.GetMostFrequentPrefixesToCount(comb, CHARS_PER_PREFIX_COUNT, MAX_PREFIXES_COUNT);
            watch.Stop();

            foreach (var keyValuePair in dict)
                Console.WriteLine(keyValuePair.Key.ToString().ToLower() + " " + keyValuePair.Value);
            Console.WriteLine(watch.ElapsedMilliseconds);
        }


    }
}