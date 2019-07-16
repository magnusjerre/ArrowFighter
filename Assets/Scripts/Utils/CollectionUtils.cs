using System.Collections.Generic;
using UnityEngine;

namespace Jerre.Utils
{
    public class CollectionUtils
    {

        public static string Stringify(string[] array)
        {
            string concat = "[";
            for (var i = 0; i < array.Length; i++)
            {
                if (i < array.Length - 1)
                {
                    concat += array[i] + ",";
                }
                else
                {
                    concat += array[i];
                }
            }
            concat += "]";

            return concat;
        }

        public static int[] ExtractRandomValues(List<int> elements, int nRandomElements)
        {
            var output = new int[nRandomElements];
            var outputIndex = 0;

            var set = new HashSet<int>();
            while (set.Count < nRandomElements)
            {
                var index = Random.Range(0, elements.Count);
                if (set.Add(elements[index]))
                {
                    output[outputIndex++] = elements[index];
                }
            }

            return output;
        }
    }
}
