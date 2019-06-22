using System.Collections.Generic;
using UnityEngine;


namespace Jerre.JPhysics
{
    public class MeshDebuggerManager : MonoBehaviour
    {
        List<LineRenderer> lineRenderers;

        public LineRenderer EdgePrfeab;

        public Transform LineRenderersParent;

        private void Awake()
        {
            lineRenderers = new List<LineRenderer>(100);
        }

        void LateUpdate()
        {
            var physicsBodies = GameObject.FindObjectsOfType<PhysicsbodyRectangular>();
            var pbLength = physicsBodies.Length;

            var nextLineRendererIndex = 0;
            for (var i = 0; i < pbLength; i++)
            {
                var body = physicsBodies[i];
                if (body.DebugMesh)
                {
                    LineRenderer lr;
                    if (nextLineRendererIndex < lineRenderers.Count)
                    {
                        lr = lineRenderers[nextLineRendererIndex];
                    } else
                    {
                        lr = Instantiate(EdgePrfeab, LineRenderersParent);
                        lineRenderers.Add(lr);
                    }

                    var edgeCoordinates = body.GetEdgeCoordinates();
                    lr.positionCount = edgeCoordinates.Length;
                    lr.SetPositions(body.GetEdgeCoordinates());
                    nextLineRendererIndex++;
                }
                if (body.DebugAABB)
                {
                    LineRenderer lr;
                    if (nextLineRendererIndex < lineRenderers.Count)
                    {
                        lr = lineRenderers[nextLineRendererIndex];
                    }
                    else
                    {
                        lr = Instantiate(EdgePrfeab, LineRenderersParent);
                        lineRenderers.Add(lr);
                    }

                    var min = body.jMeshFrameInstance.AABB.min;
                    var max = body.jMeshFrameInstance.AABB.max;

                    lr.positionCount = 5;
                    lr.SetPositions(new Vector3[] {
                        new Vector3(min.x, 1, min.z),
                        new Vector3(max.x, 1, min.z),
                        new Vector3(max.x, 1, max.z),
                        new Vector3(min.x, 1, max.z),
                        new Vector3(min.x, 1, min.z),
                    });
                    nextLineRendererIndex++;
                }
                if (body.DebugAABB && body.MeshRenderer != null)
                {
                    LineRenderer lr;
                    if (nextLineRendererIndex < lineRenderers.Count)
                    {
                        lr = lineRenderers[nextLineRendererIndex];
                    }
                    else
                    {
                        lr = Instantiate(EdgePrfeab, LineRenderersParent);
                        lineRenderers.Add(lr);
                    }

                    var min = body.MeshRenderer.bounds.min;
                    var max = body.MeshRenderer.bounds.max;

                    lr.positionCount = 5;
                    lr.SetPositions(new Vector3[] {
                        new Vector3(min.x, 1, min.z),
                        new Vector3(max.x, 1, min.z),
                        new Vector3(max.x, 1, max.z),
                        new Vector3(min.x, 1, max.z),
                        new Vector3(min.x, 1, min.z),
                    });

                    nextLineRendererIndex++;
                }
            }

            if (nextLineRendererIndex < lineRenderers.Count)
            {
                lineRenderers.RemoveRange(nextLineRendererIndex, (lineRenderers.Count - nextLineRendererIndex));
            }
        }
    }
}