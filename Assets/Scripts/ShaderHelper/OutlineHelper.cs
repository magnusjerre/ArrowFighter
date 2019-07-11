using Jerre.JColliders;
using UnityEngine;

namespace Jerre.ShaderHelper
{
    public class OutlineHelper : MonoBehaviour
    {
        public float outlineWidth = 1f;
        public float verticalOffset = -0.5f;

        void Start()
        {
            var jCollider = GetComponentInParent<JCollider>();
            Transform outlineChild = null;
            Transform overlayChild = null;

            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.name.Equals("Overlay"))
                {
                    Debug.Log("Overlay child reached");
                    overlayChild = child;
                    var childMeshFilter = child.GetComponent<MeshFilter>();
                    childMeshFilter.mesh.vertices = CalculateVerticesForOverlay(outlineWidth, jCollider.meshIdentity.EdgeVertices, jCollider.meshIdentity.EdgeOutwardNormals, childMeshFilter.mesh.vertices);
                } else if (child.name.Equals("Outline"))
                {
                    outlineChild = child;
                }
            }

            var outlineMaterial = outlineChild.GetComponent<Renderer>().material;
            var overlayMaterial = overlayChild.GetComponent<Renderer>().material;
            outlineMaterial.color = outlineMaterial.color * overlayMaterial.color;
        }

        public Vector3[] CalculateVerticesForOverlay(float outlineWidth, Vector3[] originalVertices, Vector3[] originalNormals, Vector3[] meshVertices)
        {
            var output = new Vector3[meshVertices.Length];
            for (var i = 0; i < meshVertices.Length; i++)
            {
                output[i] = meshVertices[i];
            }

            var pNormal = originalNormals[originalNormals.Length - 1];
            var nNormal = originalNormals[0];
            var v = originalVertices[0];

            var ind = GetOriginalVertexIndex(v, meshVertices);
            if (ind == -1)
            {
                Debug.Log("Couldn't find the corresponding vertex in the original mesh filter");
                return output;
            }
            var oExpandNormal = pNormal * outlineWidth + nNormal * outlineWidth;
            output[ind] = v - oExpandNormal;   // Overlay should be smaller than the outline object, therefore subtracting

            for (var i = 1; i < originalNormals.Length; i++)
            {
                var prevNormal = originalNormals[i - 1];
                var nextNormal = originalNormals[i];
                var vertex = originalVertices[i];

                var index = GetOriginalVertexIndex(vertex, meshVertices);
                if (index == -1)
                {
                    Debug.Log("Couldn't find the corresponding vertex in the original mesh filter");
                    return output;
                }


                var outlineExpandNormal = prevNormal * outlineWidth + nextNormal * outlineWidth;
                output[index] = vertex - outlineExpandNormal;   // Overlay should be smaller than the outline object, therefore subtracting
            }

            for (var i = 0; i < output.Length; i++)
            {
                var o = output[i];
                output[i] = new Vector3(o.x, -verticalOffset, o.z);
            }

            return output;
        }

        public int GetOriginalVertexIndex(Vector3 vertex, Vector3[] meshVertices)
        {
            for (var i = 0; i < meshVertices.Length; i++)
            {
                if (AreEqualIsh(vertex, meshVertices[i], 0.01f)) {
                    return i;
                }
            }
            return -1;
        }

        public static bool AreEqualIsh(Vector3 expected, Vector3 actual, float maxDiff)
        {
            if (Mathf.Abs(expected.x - actual.x) > maxDiff)
            {
                return false;               
            }
            if (Mathf.Abs(expected.y - actual.y) > maxDiff)
            {
                return false;
            }
            if (Mathf.Abs(expected.z - actual.z) > maxDiff)
            {
                return false;
            }
            return true;
        }

    }
}
