using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class UmbraOfferingPlayer : ModPlayer
    {
        public bool hasUmbraOffering = false;
        private int spawnTimer = 0;

        public override void ResetEffects()
        {
            hasUmbraOffering = false;
        }

        public override void PostUpdate()
        {
            if (!hasUmbraOffering)
                return;

            spawnTimer++;
            int interval = Main.eclipse ? 15 : 30; // every 0.25s or 0.5s

            if (spawnTimer >= interval)
            {
                spawnTimer = 0;

                Vector2 spawnPos = Player.Center + Main.rand.NextVector2Circular(240f, 240f);
                Projectile.NewProjectile(
                    Player.GetSource_FromThis(),
                    spawnPos,
                    Vector2.Zero,
                    ModContent.ProjectileType<EclipseBall>(),
                    0,
                    0,
                    Player.whoAmI
                );
            }
        }
    }
}
