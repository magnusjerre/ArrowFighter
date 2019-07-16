using System;

namespace Jerre.Utils
{
    public class ArrayUtils
    {
        public static int[] Merge(int[] arr1, int[] arr2)
        {
            var output = new int[arr1.Length + arr2.Length];

            Array.Copy(arr1, output, arr1.Length);
            Array.Copy(arr2, 0, output, arr1.Length, arr2.Length);

            return output;
        }

        public static bool Contains(int value, int[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                if (array[i] == value) return true;
            }
            return false;
        }

        public static int CountOccurrences(int value, int[] array)
        {
            var count = 0;
            for (var i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
