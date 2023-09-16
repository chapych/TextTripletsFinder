using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TextParser.Extensions
{
    public static class ConcurrentBagExtensions
    {
        public static void AddRange<T>(this ConcurrentBag<T> concurrentBag, IEnumerable<T> array)
        {
            foreach (T element in array) concurrentBag.Add(element);
        }
    }
}