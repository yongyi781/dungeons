using System;
using System.Collections.Generic;

namespace MapGenerator
{
    public static class ListExtensions
    {
        private static Random rng = new Random();

        // Randomly shuffles a list
        public static void Shuffle<T>(this IList<T> list, Random random = null)
        {
            if (random == null)
                random = rng;

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
