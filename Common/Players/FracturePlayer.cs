using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class FracturePlayer : ModPlayer
    {
        public int prismaticChargeTimer;

        public override void ResetEffects()
        {
            if (Player.HeldItem.type != ModContent.ItemType<Content.Items.Weapons.PrismaticFracture>())
            {
                prismaticChargeTimer = 0;
            }
        }
    }
}

