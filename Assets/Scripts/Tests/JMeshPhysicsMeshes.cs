using UnityEngine;

namespace Tests
{
    public class JMeshPhysicsMeshes
    {
        public static readonly Vector3[] triangle = new Vector3[] {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 1)
        };
        public static readonly int[] triangleTriangles = new int[]
        {
            0, 1, 2
        };
        public static readonly Vector3[] triangleNormals = new Vector3[]   // Starting with lower edge as first normal
        {
            new Vector3(0, 0, -1),
            new Vector3(1, 0, 0),
            new Vector3(-Mathf.Cos(45f), 0, Mathf.Cos(45f)).normalized
        };

        public static readonly Vector3[] square = new Vector3[] {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 1),
                new Vector3(0, 0, 1)
        };
        public static readonly int[] squareTriangles = new int[]
        {
            0, 1, 2,    // lower right
            2, 3, 0     // upper left
        };
        public static readonly int[] squareTrianglesDifferentTriangles = new int[]
        {
            0, 1, 3,    // lower left
            3, 1, 2     // upper right
        };
        public static readonly Vector3[] squareNormals = new Vector3[]
        {
            new Vector3(0, 0, -1),
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(-1, 0, 0)
        };

        public static readonly Vector3[] squareFive = new Vector3[] {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(0.5f, 0, 0.5f),
                new Vector3(1, 0, 1),
                new Vector3(0, 0, 1)
        };
        public static readonly int[] squareFiveTriangles = new int[]
        {
            0, 1, 2,    // lower triangle
            2, 1, 3,    // right triangle
            3, 4, 2,    // upper triangle
            2, 4, 0     // left triangle
        };
        public static readonly Vector3[] squareFiveNormals = new Vector3[]
        {
            new Vector3(0, 0, -1),
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(-1, 0, 0)
        };
    }
}