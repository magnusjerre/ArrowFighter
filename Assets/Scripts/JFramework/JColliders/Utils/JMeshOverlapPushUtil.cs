using Jerre.JColliders;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre.JPhysics
{
    public class JMeshOverlapPushUtil
    {

        public static Push CalculateMinimumPush(JMesh meshA, JMesh meshB)
        {
            // Mulig metoden bør utbedres med sjekk i alle normalretningene for alle vertices, men dette kan være dyrt?

            var pushA = CalculatePushForPoints(meshA.EdgeVertices, meshB);
            var pushB = CalculatePushForPoints(meshB.EdgeVertices, meshA);

            if (pushA.Magnitude <= pushB.Magnitude)
            {
                return pushA;
            }
            return pushB;
            //var verticesA = meshA.EdgeVertices;
            //var endA = verticesA.Length - 1;
            //var normalsEndB = meshB.EdgeOutwardNormals.Length;
            //var distancesA = new List<float[]>(normalsEndB);
            //for (var i = 0; i < endA; i++)
            //{
            //    distancesA.Add(CalculatePushLengthsForPoint(meshB, verticesA[i]));
            //}

            //var maxValuesA = FindMaxesForEachValue(distancesA);
            //foreach (var v in distancesA)
            //{
            //    Debug.Log(Logging.AsString(v));
            //}
            //var indexOfMin = IndexOfSmallestValue(maxValuesA);
            //var minDistanceA = maxValuesA[indexOfMin];
            //var directionA = meshB.EdgeOutwardNormals[indexOfMin];


            //var verticesB = meshB.EdgeVertices;
            //var endB = verticesB.Length - 1;
            //var normalsEndA = meshA.EdgeOutwardNormals.Length;
            //var distancesB = new List<float[]>(normalsEndA);
            //for (var i = 0; i < endB; i++)
            //{
            //    distancesB.Add(CalculatePushLengthsForPoint(meshA, verticesB[i]));
            //}

            //Debug.Log("B");
            //foreach (var v in distancesB)
            //{
            //    Debug.Log(Logging.AsString(v));
            //}
            //var maxValuesB = FindMaxesForEachValue(distancesB);
            //var indexOfMinB = IndexOfSmallestValue(maxValuesB);
            //var minDistanceB = maxValuesB[indexOfMinB];
            //var directionB = meshB.EdgeOutwardNormals[indexOfMinB];

            //Debug.Log("maxValuesA: " + Logging.AsString(maxValuesA));
            //Debug.Log("maxValuesB: " + Logging.AsString(maxValuesB));
            //Debug.Log("minDistanceA: " + minDistanceA + ", directionA: " + directionA + ": minDistanceB: " + minDistanceB + ", directionB: " + directionB);
            //Debug.Log("verticesA: " + verticesA.Length + ", verticesB: " + verticesB.Length);

            //if (minDistanceA <= minDistanceB)
            //{
            //    return new Push(directionA, minDistanceA);
            //}
            //return new Push(directionB, minDistanceB);






            // FV (fevest vertices), MV (most vertices)
            // Velg mesh med færrest vertices, FV 
            // For hver vertex i FV, beregn dytte-avstand i retningene til hver av normalene på MV. Dersom en vertex allerede er utenfor, gi negativ dyttestørrelse
            // Lagre avstander og retninger i FV-pushes for alle vertex i FV
            // Velg så maks-avstand i hver av retningene, og lagre i en FVMax array
            // Velg så minste verdi for hver av retningene, lagre resultatet av retning og størrelse i FVBest

            // Velg mesh med flest vertices, MV
            // For hver vertex i MV, beregn dytte-avstand i retningene til hver av normalene på FV. Dersom en vertex allerede er utenfor, gi negativ dyttestørrelse
            // Lagre avstander og retninger i MV-pushes for alle vertex i MV
            // Velg så maks-avstand i hver av retningene, og lagre i en MVMax array
            // Velg så minste verdi for hver av retningene, lagre resultatet av retning og størrelse i MVBest

            // Velg så minste avstand av FVBest og MVBest
        }

        public static Push CalculatePushForPoints(Vector3[] points, JMesh mesh)
        {
            var vertices = points;
            var end = points.Length - 1;
            var normals = mesh.EdgeOutwardNormals.Length;
            var distancesA = new List<float[]>(normals);
            for (var i = 0; i < end; i++)
            {
                distancesA.Add(CalculatePushLengthsForPoint(mesh, vertices[i]));
            }

            var maxValues = FindMaxesForEachValue(distancesA);
            var indexOfFirstMinValue = IndexOfSmallestValue(maxValues);
            var minPushDistance = maxValues[indexOfFirstMinValue];
            var pushDirection = mesh.EdgeOutwardNormals[indexOfFirstMinValue];

            return new Push(pushDirection, minPushDistance);
        }

        public static float[] CalculatePushLengthsForPoint(JMesh meshA, Vector3 vertex)
        {
            var aVertices = meshA.EdgeVertices;
            var aNormals = meshA.EdgeOutwardNormals;
            var aNormalsLength = aNormals.Length;
            var pushLengths = new float[aNormalsLength];
            for (var a = 0; a < aNormalsLength; a++)
            {
                var PAVector = (aVertices[a] - vertex);
                var normal = aNormals[a];
                var distance = Vector3.Dot(normal, PAVector);
                pushLengths[a] = distance;
                //Debug.Log("vertex: " + vertex + ", PAVector: " + PAVector + ", normal: " + normal + ", distance: " + distance);
            }

            return pushLengths;
        }

        public static float[] FindMaxesForEachValue(List<float[]> directionMagnitudes)
        {
            float[] currentMaxes = new float[directionMagnitudes[0].Length];
            for (var i = 0; i < currentMaxes.Length; i++)
            {
                currentMaxes[i] = float.MinValue;
            }

            for (var i = 0; i < directionMagnitudes.Count; i++)
            {
                var magnitudes = directionMagnitudes[i];
                for (var j = 0; j < magnitudes.Length; j++)
                {
                    if (magnitudes[j] > currentMaxes[j])
                    {
                        currentMaxes[j] = magnitudes[j];
                    }
                }
            }

            return currentMaxes;
        }

        public static int IndexOfSmallestValue(float[] values)
        {
            var indexOfSmallest = 0;
            var smallestValue = values[indexOfSmallest];
            for (var i = 1; i < values.Length; i++)
            {
                if (values[i] < smallestValue)
                {
                    smallestValue = values[i];
                    indexOfSmallest = i;
                }
            }

            return indexOfSmallest;
        }
    }
}