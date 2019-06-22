namespace Jerre.JPhysics
{
    public class JLayerMaskUtil
    {
        private static int PlayerMask = (int)JLayer.BULLET | (int)JLayer.SCENERY;
        private static int BulletMask = (int)JLayer.PLAYER | (int)JLayer.SCENERY;
        private static int SceneryMask = (int)JLayer.PLAYER | (int)JLayer.BULLET;

        public static int GetLayerMask(JLayer mask)
        {
            switch(mask)
            {
                case JLayer.PLAYER: return PlayerMask;
                case JLayer.BULLET: return BulletMask;
                case JLayer.SCENERY: return SceneryMask;
                default: return 0;
            }
        }

        public static bool MaskCheck(int mask, JLayer layer)
        {
            return (mask & (int)layer) > 0;
        }
    }
}
