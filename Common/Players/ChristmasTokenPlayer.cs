using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class ChristmasTokenPlayer : ModPlayer
    {
        public bool hasChristmasToken = false;
        private int orbTimer = 0;

        public override void ResetEffects()
        {
            hasChristmasToken = false;
        }

        public override void PostUpdate()
        {
            if (!hasChristmasToken)
                return;

            orbTimer++;
            if (orbTimer >= 30)
            {
                orbTimer = 0;
                Vector2 spawn = Player.Center + Main.rand.NextVector2Circular(320f, 320f);
                Projectile.NewProjectile(Player.GetSource_FromThis(), spawn, Vector2.Zero,
                    ModContent.ProjectileType<ChristmasOrb>(), 0, 0, Player.whoAmI);
            }
        }
    }
}
