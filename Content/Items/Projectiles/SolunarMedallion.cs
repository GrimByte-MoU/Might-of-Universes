using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    // ai[0] = initial phase (0 or π)
    // localAI[0] anchorX, localAI[1] anchorY, localAI[2] initialized flag (0/1)
    public class SolunarMedallion : ModProjectile
    {
        private const float OrbitRadius = 150f;
        private const float OrbitSpeed = 0.035f;
        private const int   Lifetime   = 240;

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
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

            // Initialize anchor only once
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
            //Projectile.rotation += 0.18f;

            if (Main.rand.NextBool(10))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    Main.rand.NextBool() ? Terraria.ID.DustID.PurpleTorch : Terraria.ID.DustID.OrangeTorch,
                    0f, 0f, 140, default, 0.9f);
                Main.dust[d].noGravity = true;
            }
        }
    }
}
