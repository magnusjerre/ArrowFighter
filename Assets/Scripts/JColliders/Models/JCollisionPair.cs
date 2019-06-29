using System.Collections.Generic;

namespace Jerre.JColliders
{
    public struct JCollisionPair
    {
        public JCollider body1;
        public JCollider body2;

        public JCollisionPair(JCollider body1, JCollider body2)
        {
            this.body1 = body1;
            this.body2 = body2;
        }

        public override bool Equals(object obj)
        {
            var pair = (JCollisionPair)obj;
            return ((pair.body1 == body1 && pair.body2 == body2) || (pair.body1 == body2 && pair.body2 == body1));
        }

        public override int GetHashCode()
        {
            return EqualityComparer<JCollider>.Default.GetHashCode(body1) + EqualityComparer<JCollider>.Default.GetHashCode(body2);
        }
    }
}
