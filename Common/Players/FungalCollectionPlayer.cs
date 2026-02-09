using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class FungalCollectionPlayer : ModPlayer
    {
        public bool fungalCollection;

        private int sporeTimer;

        public override void ResetEffects()
        {
            fungalCollection = false;
        }

        public override void PostUpdate()
        {
            if (fungalCollection)
            {
                sporeTimer++;
                if (sporeTimer >= 60)
                {
                    sporeTimer = 0;
                    Vector2 spawnPosition = Player.Center + new Vector2(Main.rand.Next(-240, 240), Main.rand.Next(-240, 240));
                    if (Vector2.Distance(spawnPosition, Player.Center) <= 240f)
                    {
                        Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPosition, Vector2.Zero,
                            ModContent.ProjectileType<MushroomSporeStrong>(), 25, 0f, Player.whoAmI);
                    }
                }
            }
        }
    }
}
