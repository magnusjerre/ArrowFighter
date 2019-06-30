using Jerre.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre.JColliders
{
    public class JColliderDebuggerManager : MonoBehaviour
    {

        List<LineRenderer> lineRenderers;

        public LineRenderer EdgePrfeab;

        public Transform LineRenderersParent;

        public RectTransform CollisionMapDebugView;
        public bool DebugCollisionMap = false;

        private void Awake()
        {
            lineRenderers = new List<LineRenderer>(100);
        }

        void LateUpdate()
        {
            DebugAABBAndEdges();
            if (DebugCollisionMap)
            {
                DebugCollisionMaps();
            }
        }

        private void DebugAABBAndEdges()
        {
            var physicsBodies = JColliderContainer.INSTANCE.ActiveColliders();
            var pbLength = physicsBodies.Count;


            var nextLineRendererIndex = 0;
            for (var i = 0; i < pbLength; i++)
            {
                var body = physicsBodies[i];
                if (body.debugMesh)
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

                    var edgeCoordinates = body.meshFrame.EdgeVertices;
                    lr.positionCount = edgeCoordinates.Length;
                    lr.SetPositions(body.meshFrame.EdgeVertices);
                    nextLineRendererIndex++;
                }
                if (body.debugAABB)
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

                    var min = body.meshFrame.AABB.min;
                    var max = body.meshFrame.AABB.max;

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

        private void DebugCollisionMaps()
        {
            var queue = JColliderManager.collisionMapQueue;
            queue.ResetQueueIteration();

            var childIndex = 0;

            for (var i = 0; i < CollisionMapDebugView.childCount; i++)
            {
                CollisionMapDebugView.GetChild(i).gameObject.SetActive(false);
            }

            while (queue.HasNext() && childIndex < CollisionMapDebugView.childCount)
            {
                var map = queue.Next();
                if (map.bodies != null) {
                    var childRect = CollisionMapDebugView.GetChild(childIndex).GetComponent<RectTransform>();
                    CameraHelper.PaintBoundsOnScreen(map.bounds, childRect);
                }

                childIndex++;
            }

        }
    }
}
