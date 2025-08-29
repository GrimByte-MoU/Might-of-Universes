using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class VolatileSandExplosion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.timeLeft = 2;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void AI()
        {
            if (Projectile.owner != Main.myPlayer)
                return;

            NPC target = Main.npc[(int)Projectile.ai[0]];
            if (!target.active)
                return;

            // Visual burst
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Projectile.Center, 0, 0, DustID.Sandnado, Scale: 1.5f);
            }

            // Spawn 6 upward sand needles
            for (int i = 0; i < 6; i++)
            {
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-8f, -5f));

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    velocity,
                    ModContent.ProjectileType<VolatileSandShard>(),
                    10,
                    0f,
                    Projectile.owner
                );
            }
        }
    }
}
