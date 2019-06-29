using System.Collections.Generic;
using UnityEngine;

namespace Jerre.JColliders
{
    public class JColliderContainer
    {
        private static JColliderContainer instance = null;
        public static JColliderContainer INSTANCE
        {
            get
            {
                if (instance == null)
                {
                    instance = new JColliderContainer();
                }
                return instance;
            }
        }
        private JColliderContainer()
        {
            allCollidersInScene = new HashSet<JCollider>();
        }


        private HashSet<JCollider> allCollidersInScene;


        public bool Add(JCollider collider)
        {
            return allCollidersInScene.Add(collider);
        }

        public bool Remove(JCollider collider)
        {
            return allCollidersInScene.Remove(collider);
        }

        public List<JCollider> ActiveColliders()
        {
            var output = new List<JCollider>(allCollidersInScene.Count);

            foreach (JCollider collider in allCollidersInScene)
            {
                if (collider.enabled && collider.gameObject.activeSelf)
                {
                    output.Add(collider);
                }
            }
            return output;
        }

        public void ClearColliderList()
        {
            allCollidersInScene.Clear();
        }

        public int ColliderCount()
        {
            return allCollidersInScene.Count;
        }
    }

}
