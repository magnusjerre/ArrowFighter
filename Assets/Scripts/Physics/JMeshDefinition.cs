using System;
using System.Collections.Generic;
using UnityEngine;

public class JMeshDefintion
{
    public Vector3[] Vertices;
    public Vector3[] Normals;
    public int[] EdgeIndices;

    public JMeshDefintion(Vector3[] vertices, Vector3[] normals, int[] edgeIndeces)
    {
        Vertices = vertices;
        Normals = normals;
        EdgeIndices = edgeIndeces;
    }

    static void AddEdgeOrMoveToInnerEdges(Edge edge, List<Edge> edges, List<Edge> innerEdges)
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

    public static JMeshDefintion FromMesh(Mesh mesh)
    {
        var vertices = mesh.vertices;
        var verticesLength = vertices.Length;
        var triangles = mesh.triangles;
        var trianglesCount = triangles.Length / 3;
        var edges = new List<Edge>();
        var innerEdges = new List<Edge>();

        for (var i = 0; i < trianglesCount; i++)
        {
            int a = triangles[i * 3];
            int b = triangles[i * 3 + 1];
            int c = triangles[i * 3 + 2];

            var edgeAB = new Edge(a, b);
            var edgeBC = new Edge(b, c);
            var edgeCA = new Edge(c, a);

            AddEdgeOrMoveToInnerEdges(edgeAB, edges, innerEdges);
            AddEdgeOrMoveToInnerEdges(edgeBC, edges, innerEdges);
            AddEdgeOrMoveToInnerEdges(edgeCA, edges, innerEdges);
        }

        var currentEdge = edges[0];
        var valueOfStart = currentEdge.a;
        var currentEndValue = currentEdge.b;
        var outputEdges = new List<int>();
        outputEdges.Add(valueOfStart);
        outputEdges.Add(currentEndValue);
        int count = 1;
        while (count < edges.Count)
        {
            currentEdge = edges.Find(e => (e.a == currentEndValue || e.b == currentEndValue) && (e.a != valueOfStart && e.b != valueOfStart));
            valueOfStart = currentEndValue;
            currentEndValue = currentEdge.a == valueOfStart ? currentEdge.b : currentEdge.a;
            outputEdges.Add(currentEndValue);

            count++;
        }

        return new JMeshDefintion(vertices, mesh.normals, outputEdges.ToArray());
    }

    struct Edge : IEquatable<Edge>
    {
        public int a;
        public int b;
        public int hashCode;

        public Edge(int a, int b)
        {
            int max = a > b ? a : b;
            int min = a < b ? a : b;
            this.a = a;
            this.b = b;
            hashCode = 1000 * max + min;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Edge))
            {
                return false;
            }

            var edge = (Edge)obj;
            return hashCode == edge.hashCode;
        }

        public bool Equals(Edge other)
        {
            return other.hashCode == hashCode;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}
