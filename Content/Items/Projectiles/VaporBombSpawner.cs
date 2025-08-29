using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class VaporBombSpawner : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.None;

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 3600; // 1 minute lifespan
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.owner != Main.myPlayer) return;

            if (++Projectile.localAI[0] >= 30f)
            {
                Projectile.localAI[0] = 0f;

                // spawn a Vapor Bomb
                Vector2 spawnPos = Projectile.Center + Main.rand.NextVector2Circular(240f, 240f); // ~15 tile radius
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    spawnPos,
                    Vector2.Zero,
                    ModContent.ProjectileType<VaporBomb>(),
                    Projectile.originalDamage,
                    0f,
                    Projectile.owner
                );
            }

            Projectile.Center = Main.player[Projectile.owner].Center;
        }
    }
}
