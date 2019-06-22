using UnityEngine;
using System.Collections;

namespace Jerre.JPhysics
{
    public struct PushResult
    {
        public bool CanPush;
        public Push push;
        public bool APushB;

        public PushResult(bool canPush, Push push, bool aPushB)
        {
            CanPush = canPush;
            this.push = push;
            APushB = aPushB;
        }

        public override string ToString()
        {
            return "canPush: " + CanPush + ", push: " + push + ", aPushB: " + APushB;
        }
    }
}
