using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorBuilder
{
    public static class Extensions
    {
        public static Random rand = new Random();
        public static string Foo(this string s, int count)
        {
            return count.ToString();
        }

        public static T PopRandom<T>(this List<T> list)
        {
            var randomIndex = rand.Next(list.Count() - 1);
            var element = list[randomIndex];
            list.RemoveAt(randomIndex);
            return element;
        }

        public static T[,] Fill<T>(this T[,] array, T value)
        {
            for (int i = 0; i < array.Rank; i++)
            {
                for (int j = 0; j < array.Length; j++)
                {
                    array.SetValue(value, i, j);
                }
            }
            return array;
        }
    }
}
