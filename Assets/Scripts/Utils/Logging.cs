using UnityEngine;
using System.Collections;

namespace Jerre.Utils
{
    public class Logging
    {
        public static string AsString(Vector3[] vectors)
        {
            string str = "[\n";
            for (var i = 0; i < vectors.Length; i++)
            {
                str += "i: " + i + ", {x: " + vectors[i].x + ", y: " + vectors[i].y + ", z: " + vectors[i].z + "}";
                if (i < vectors.Length - 1)
                {
                    str += ",\n";
                }
            }
            return str + "]";
        }

        public static string AsString(int[] values)
        {
            string str = "[";
            for (var i = 0; i < values.Length; i++)
            {
                str += values[i] ;
                if (i < values.Length - 1)
                {
                    str += ",";
                }
            }
            return str + "]";
        }

        public static string AsString(float[] values)
        {
            string str = "[";
            for (var i = 0; i < values.Length; i++)
            {
                str += values[i];
                if (i < values.Length - 1)
                {
                    str += ",";
                }
            }
            return str + "]";
        }
    }
}
