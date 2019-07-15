namespace Jerre.JColliders
{
    public class JLayerMaskUtil
    {
        private static int PlayerMask = (int)JLayer.BULLET | (int)JLayer.SCENERY | (int)JLayer.PICKUP;
        private static int BulletMask = (int)JLayer.PLAYER | (int)JLayer.SCENERY;
        private static int SceneryMask = (int)JLayer.PLAYER | (int)JLayer.BULLET;
        private static int PickupMask = (int)JLayer.PLAYER;

        public static int GetLayerMask(JLayer mask)
        {
            switch(mask)
            {
                case JLayer.PLAYER: return PlayerMask;
                case JLayer.BULLET: return BulletMask;
                case JLayer.SCENERY: return SceneryMask;
                case JLayer.PICKUP: return PickupMask;
                default: return 0;
            }
        }

        public static bool MaskCheck(int mask, JLayer layer)
        {
            return (mask & (int)layer) > 0;
        }
    }
}
