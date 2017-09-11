using System;
using System.Collections.Generic;
using System.Linq;

namespace RouteFinderLibrary.Tests
{
    internal static class EnumerableEx
    {
        internal static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, IRandomGenerator randomGenerator = null)
        {
            randomGenerator = randomGenerator ?? new DefaultRandomGenerator();
            var list = source.ToList();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                int k = randomGenerator.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }

    internal interface IRandomGenerator
    {
        int Next(int maxValue);
    }

    internal class DefaultRandomGenerator : IRandomGenerator
    {
        private static readonly Random Random = new Random();

        public int Next(int maxValue)
        {
            return Random.Next(maxValue);
        }
    }

    internal class DelegateRandomGenerator : IRandomGenerator
    {
        private readonly Func<int, int> _generator;

        public DelegateRandomGenerator(Func<int, int> generator)
        {
            _generator = generator;
        }

        public int Next(int maxValue)
        {
            return _generator(maxValue);
        }
    }
}
