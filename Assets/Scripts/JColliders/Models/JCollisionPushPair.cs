namespace Jerre.JColliders
{
    public struct JCollisionPushPair
    {
        public JCollider body1;
        public JCollider body2;
        public Push push;

        public JCollisionPushPair(JCollider body1, JCollider body2, Push push)
        {
            this.body1 = body1;
            this.body2 = body2;
            this.push = push;
        }
    }
}
