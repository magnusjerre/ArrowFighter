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
    }
}
