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
          ReadOnlyMemory<char> word = "the".AsMemory();
          var dictionary = new Dictionary<ReadOnlyMemory<char>, int>(new ReadOnlyMemoryComparer());
          dictionary[word] = 1;
          //dictionary.AddOrUpdate(word.Slice(0,3), addValue: 1, updateValueFactory: (_, value) => ++value);
          Console.WriteLine(dictionary.ContainsKey("tHe".AsMemory()));
          Console.ReadLine();
           char[] separatingChars = {'.', '!', '?', ';', ':', '(', ')',' ', ',', '"', '\n', '-', '\'', '“', '”'};

            var textParser = new TextParser(separatingChars);
            var prefixFinder = new PrefixFinder();
            var watch = new Stopwatch();
            string path = "D:/c#/TextAnalysis.csproj/HarryPotterText.txt";

            watch.Start();
            var combinations = new PrefixFinder_new().Find(path, separatingChars);
            watch.Stop();

            foreach (var keyValuePair in combinations)
                Console.WriteLine(keyValuePair.Key + " " + keyValuePair.Value);
            Console.WriteLine(watch.ElapsedMilliseconds);
        }
    }
}