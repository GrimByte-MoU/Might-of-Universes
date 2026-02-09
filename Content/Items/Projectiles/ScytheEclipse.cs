using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ScytheEclipse : MoUProjectile
    {
        private const float OrbitRadius = 140f;
        private const float OrbitSpeed = 0.04f;
        private const int Lifetime = 240;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = Lifetime;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead)
            {
                Projectile.Kill();
                return;
            }

            if (Projectile.localAI[2] == 0f)
            {
                Projectile.localAI[0] = Projectile.Center.X;
                Projectile.localAI[1] = Projectile.Center.Y;
                Projectile.localAI[2] = 1f;
            }

            Vector2 anchor = new(Projectile.localAI[0], Projectile.localAI[1]);
            float elapsed = Lifetime - Projectile.timeLeft;
            float angle = Projectile.ai[0] + elapsed * OrbitSpeed;

            Projectile.Center = anchor + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * OrbitRadius;
            Projectile.rotation += 0.2f;

            if (Main.rand.NextBool(6))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.AncientLight, 0f, 0f, 150, default, 0.9f);
                Main.dust[d].noGravity = true;
            }
        }
    }
}