using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre.JPhysics
{
    public struct JMesh
    {
        public Vector3[] EdgeVertices;
        public Vector3[] EdgeOutwardNormals;

        public JMesh(Vector3[] edgeVertices, Vector3[] edgeOutwardNormals)
        {
            EdgeVertices = edgeVertices;
            EdgeOutwardNormals = edgeOutwardNormals;
        }

        static void AddEdgeOrMoveToInnerEdges(EdgeV2 edge, List<EdgeV2> edges, List<EdgeV2> innerEdges)
        {
            if (!innerEdges.Contains(edge))
            {
                if (edges.Contains(edge))
                {
                    innerEdges.Add(edge);
                    edges.Remove(edge);
                }
                else
                {
                    edges.Add(edge);
                }
            }
        }

        public static Vector3[] ExtractEdgeVertices(Vector3[] vertices, int[] triangles)
        {
            var verticesLength = vertices.Length;
            var trianglesCount = triangles.Length / 3;
            var edges = new List<EdgeV2>();
            var innerEdges = new List<EdgeV2>();

            for (var i = 0; i < trianglesCount; i++)
            {
                int a = triangles[i * 3];
                int b = triangles[i * 3 + 1];
                int c = triangles[i * 3 + 2];

                var edgeAB = new EdgeV2(a, b);
                var edgeBC = new EdgeV2(b, c);
                var edgeCA = new EdgeV2(c, a);

                AddEdgeOrMoveToInnerEdges(edgeAB, edges, innerEdges);
                AddEdgeOrMoveToInnerEdges(edgeBC, edges, innerEdges);
                AddEdgeOrMoveToInnerEdges(edgeCA, edges, innerEdges);
            }

            var currentEdge = edges[0];
            var valueOfStart = currentEdge.a;
            var currentEndValue = currentEdge.b;
            var indexOfEdgeStart = new List<int>();
            indexOfEdgeStart.Add(valueOfStart);
            indexOfEdgeStart.Add(currentEndValue);
            int count = 1;
            while (count < edges.Count)
            {
                currentEdge = edges.Find(e => (e.a == currentEndValue || e.b == currentEndValue) && (e.a != valueOfStart && e.b != valueOfStart));
                valueOfStart = currentEndValue;
                currentEndValue = currentEdge.a == valueOfStart ? currentEdge.b : currentEdge.a;
                indexOfEdgeStart.Add(currentEndValue);

                count++;
            }

            Vector3[] outputVertices = new Vector3[indexOfEdgeStart.Count];
            for (var i = 0; i < outputVertices.Length; i++)
            {
                outputVertices[i] = vertices[indexOfEdgeStart[i]];
            }

            return outputVertices;
        }

        public static Vector3[] CalculateOutwardNormals(Vector3[] edgePointsSorted)
        {
            Vector3 edgeA = (edgePointsSorted[1] - edgePointsSorted[0]).normalized;
            Vector3 edgeB = (edgePointsSorted[2] - edgePointsSorted[1]).normalized;

            Vector3 normal = new Vector3(-1, 1, 1);
            Vector3 normalInv = new Vector3(1, 1, -1);
            Vector3 normalEdgeA = new Vector3(edgeA.z * normal.x, edgeA.y * normal.y, edgeA.x * normal.z);
            Vector3 normalInvEdgeA = new Vector3(edgeA.z * normalInv.x, edgeA.y * normalInv.y, edgeA.x * normalInv.z);

            var normalAngle = Vector3.Angle(normalEdgeA, edgeB);
            var normalInvAngle = Vector3.Angle(normalInvEdgeA, edgeB);

            Vector3 normalMultiplier = normalAngle > normalInvAngle ? normal : normalInv;

            var outputNormals = new Vector3[edgePointsSorted.Length - 1];
            var end = outputNormals.Length;
            for (var i = 0; i < end; i++)
            {
                var edgeDirection = (edgePointsSorted[i + 1] - edgePointsSorted[i]).normalized;
                outputNormals[i] = new Vector3(edgeDirection.z * normalMultiplier.x, edgeDirection.y * normalMultiplier.y, edgeDirection.x * normalMultiplier.z);
            }

            return outputNormals;
        }

        public static JMesh CalculateJMesh(Vector3[] allVertices, int[] triangles)
        {
            var edgeVertices = ExtractEdgeVertices(allVertices, triangles);
            var outwardNormals = CalculateOutwardNormals(edgeVertices);
            return new JMesh(edgeVertices, outwardNormals);
        }

        struct EdgeV2 : IEquatable<EdgeV2>
        {
            public int a;
            public int b;
            public int hashCode;

            public EdgeV2(int a, int b)
            {
                int max = a > b ? a : b;
                int min = a < b ? a : b;
                this.a = a;
                this.b = b;
                hashCode = 1000 * max + min;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is EdgeV2))
                {
                    return false;
                }

                var edge = (EdgeV2)obj;
                return hashCode == edge.hashCode;
            }

            public bool Equals(EdgeV2 other)
            {
                return other.hashCode == hashCode;
            }

            public override int GetHashCode()
            {
                return hashCode;
            }
        }
    }
}
