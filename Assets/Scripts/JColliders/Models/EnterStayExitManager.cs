using UnityEngine;
using System.Collections.Generic;

namespace Jerre.JColliders
{
    public class EnterStayExitManager
    {
        private HashSet<JCollisionPair> PreviousFrame = new HashSet<JCollisionPair>();
        private HashSet<JCollisionPair> CurrentFrame = new HashSet<JCollisionPair>();


        public void Add(JCollisionPair element)
        {
            CurrentFrame.Add(element);
        }

        public void CompleteFrame()
        {
            foreach (var element in PreviousFrame)
            {
                if (CurrentFrame.Contains(element))
                {
                    if (element.body1 != null && element.body2 != null)
                    {
                        element.body1.OnJCollsionStay(element.body2);
                        element.body2.OnJCollsionStay(element.body1);
                    }
                }
                else
                {
                    if (element.body1 != null && element.body2 != null)
                    {
                        element.body1.OnJCollsionExit(element.body2);
                        element.body2.OnJCollsionExit(element.body1);
                    }
                }
            }

            foreach (var element in CurrentFrame)
            {
                if (!PreviousFrame.Contains(element))
                {
                    if (element.body1 != null && element.body2 != null)
                    {
                        element.body1.OnJCollisionEnter(element.body2);
                        element.body2.OnJCollisionEnter(element.body1);
                    }
                }
            }

            PreviousFrame = CurrentFrame;
            CurrentFrame = new HashSet<JCollisionPair>();
        }
    }
}
