using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static void Fill<T>(this T[][] array, T value)
        {
            var width = array[0].Length;
            var height = array.Length;

            var row = Enumerable.Repeat<T>(value, width).ToArray();
            for (int i = 0; i < height; i++)
            {
                array.SetValue(Enumerable.Repeat<T>(value, width).ToArray(), i);
            }
        }

        /// <summary>
        /// Fill an array with single value or repeating values. <para/>
        /// This was pinched from https://github.com/mykohsu/Extensions/blob/master/ArrayExtensions.cs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destinationArray"></param>
        /// <param name="value"></param>
        public static void Fill<T>(this T[] destinationArray, params T[] value)
        {
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }

            if (value.Length >= destinationArray.Length)
            {
                throw new ArgumentException("Length of value array must be less than length of destination");
            }

            // set the initial array value
            Array.Copy(value, destinationArray, value.Length);

            int arrayToFillHalfLength = destinationArray.Length / 2;
            int copyLength;

            // Fill up to half of the array.
            for (copyLength = value.Length; copyLength < arrayToFillHalfLength; copyLength <<= 1)
            {
                Array.Copy(destinationArray, 0, destinationArray, copyLength, copyLength);
            }

            // Fill the rest.
            Array.Copy(destinationArray, 0, destinationArray, copyLength, destinationArray.Length - copyLength);
        }
    }
}
