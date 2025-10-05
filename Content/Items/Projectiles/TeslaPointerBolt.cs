using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TeslaPointerBolt : ModProjectile
    {
        // ai0 = amplitude (pixels)
        // ai1 = direction (+1 or -1)
        // ai2 = intended lifetime ticks (so math stays stable if you tweak timeLeft)
        // localAI[0] initialization flag
        // localAI[1] = stored startX
        // localAI[2] = stored startY
        // localAI[3] = ticks elapsed

        private Vector2 forward;
        private Vector2 perp;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 36;
            Projectile.alpha = 0;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false; // disable to ensure smooth parametric arc
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                Projectile.localAI[1] = Projectile.Center.X;
                Projectile.localAI[2] = Projectile.Center.Y;
                forward = Projectile.velocity.SafeNormalize(Vector2.UnitX);
                perp = forward.RotatedBy(MathHelper.PiOver2);
                // lock forward speed magnitude
                Projectile.localAI[3] = 0f;
            }

            float amplitude = Projectile.ai[0];
            float dir = Math.Sign(Projectile.ai[1]);
            if (dir == 0) dir = 1f;
            float intendedLife = Projectile.ai[2] > 0 ? Projectile.ai[2] : 36f;

            // Progress
            float t = Projectile.localAI[3] / intendedLife; // 0..1
            Projectile.localAI[3]++;

            t = MathHelper.Clamp(t, 0f, 1f);

            // Base forward distance (constant speed)
            float forwardSpeed = 16f; // matches item shoot speed; you can pass via ai if you want
            float forwardDist = forwardSpeed * (Projectile.localAI[3] - 1);

            // Lateral displacement: A * sin(pi * t)
            float lateral = amplitude * MathF.Sin(MathF.PI * t) * dir;

            Vector2 start = new Vector2(Projectile.localAI[1], Projectile.localAI[2]);
            Vector2 targetPos = start + forward * forwardDist + perp * lateral;

            // Derive velocity so Terraria collision / damage timing stays consistent
            Projectile.velocity = targetPos - Projectile.Center;
            Projectile.Center = targetPos;

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(2))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Electric, 0f, 0f, 150, default, 1f);
                Main.dust[d].velocity *= 0.4f;
                Main.dust[d].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Electrified, 120);
        }
    }
}