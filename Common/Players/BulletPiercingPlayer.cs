using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class BulletPiercingPlayer : ModPlayer
    {
        public int bulletPiercing;

        public override void ResetEffects()
        {
            bulletPiercing = 0;
        }
    }
}
