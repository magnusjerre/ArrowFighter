namespace Jerre.JColliders
{
    public struct JCollisionPair
    {
        public JCollider body1;
        public JCollider body2;

        private int IdHashCode;
        private const long IdMultiplierFactor = 10000000;   // Number chosen slightly at random

        public JCollisionPair(JCollider body1, JCollider body2)
        {
            this.body1 = body1;
            this.body2 = body2;

            if (body1.IdGenerated < body2.IdGenerated)
            {
                IdHashCode = (body1.IdGenerated + IdMultiplierFactor * body2.IdGenerated).GetHashCode();
            }
            else
            {
                IdHashCode = (body2.IdGenerated + IdMultiplierFactor * body1.IdGenerated).GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            var pair = (JCollisionPair)obj;
            return ((pair.body1 == body1 && pair.body2 == body2) || (pair.body1 == body2 && pair.body2 == body1));
        }

        public override int GetHashCode()
        {
            return IdHashCode;
        }
    }
}
