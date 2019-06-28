using UnityEngine;
using System.Collections;

namespace Jerre.JPhysics
{
    public struct PushResult
    {
        public bool CanPush;
        public Push push;
        public bool APushB;
        public JMesh Pusher, Pushee;

        public PushResult(bool canPush, Push push, bool aPushB)
        {
            CanPush = canPush;
            this.push = push;
            APushB = aPushB;
            Pusher = default(JMesh);
            Pushee = default(JMesh);
        }

        public PushResult(bool canPush, Push push, JMesh pusher, JMesh pushee)
        {
            CanPush = canPush;
            this.push = push;
            APushB = false;
            Pusher = pusher;
            Pushee = pushee;
        }

        public PushResult(bool canPush, Push push, bool aPushB, JMesh pusher, JMesh pushee)
        {
            CanPush = canPush;
            this.push = push;
            APushB = aPushB;
            Pusher = pusher;
            Pushee = pushee;
        }



        public override string ToString()
        {
            return "canPush: " + CanPush + ", push: " + push + ", aPushB: " + APushB;
        }
    }
}
