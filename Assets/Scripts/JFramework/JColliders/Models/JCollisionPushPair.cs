namespace Jerre.JColliders
{
    public struct JCollisionPushPair
    {
        public JCollider pushable;
        public JCollider pushingFrom;
        public Push push;

        public JCollisionPushPair(JCollider collider, JCollider pushingFrom, Push push)
        {
            this.pushable = collider;
            this.pushingFrom = pushingFrom;
            this.push = push;
        }
    }
}
