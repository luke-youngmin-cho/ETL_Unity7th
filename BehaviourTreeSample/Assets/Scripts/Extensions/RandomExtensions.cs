using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class RandomExtensions
    {
        public static void RandomShuffle<T>(this Random random, T[] array)
        {
            int n = array.Length;

            while (n > 1)
            {
                int m = random.Next(n--);
                T temp = array[n];
                array[n] = array[m];
                array[m] = temp;
            }
        }

        public static void Shuffle<T>(this Random random, List<T> list)
        {
            int n = list.Count;

            while (n > 1)
            {
                int m = random.Next(n--);
                T temp = list[n];
                list[n] = list[m];
                list[m] = temp;
            }
        }
    }
}
