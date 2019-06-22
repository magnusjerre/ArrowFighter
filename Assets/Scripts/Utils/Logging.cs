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
    }
}
