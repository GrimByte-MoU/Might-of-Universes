using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TwirlyWhirly : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600; // 10 seconds max lifespan
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        private bool hasFoundTarget = false;
        private const float DetectionRange = 300f; // ~18 tiles
        private const float HomingStrength = 0.3f;
        private const float MaxSpeed = 16f;

        public override void AI()
        {
            // Spinning visual effect
            //Projectile.rotation += 0.3f;

            // Emit glow particles
            if (Main.rand.NextBool(3))
            {
                Color dustColor = Main.rand.Next(3) switch
                {
                    0 => new Color(255, 200, 100), // Gold
                    1 => new Color(200, 200, 200), // Silver
                    _ => new Color(150, 100, 50)   // Bronze
                };

                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Copper, 0f, 0f, 100, dustColor, 0.8f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            // Lighting
            Lighting.AddLight(Projectile.Center, 0.8f, 0.7f, 0.4f);

            // Find nearest enemy
            NPC target = FindNearestEnemy();

            if (target != null)
            {
                hasFoundTarget = true;

                // Home towards target
                Vector2 direction = target.Center - Projectile.Center;
                direction.Normalize();

                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * MaxSpeed, HomingStrength);
            }
            else if (!hasFoundTarget)
            {
                Projectile.velocity *= 0.98f;
                Projectile.velocity.X += Main.rand.NextFloat(-0.2f, 0.2f);
                Projectile.velocity.Y += Main.rand.NextFloat(-0.2f, 0.2f);
            }
            else
            {
                Projectile.velocity *= 0.99f;
            }
        }

        private NPC FindNearestEnemy()
        {
            NPC closestNPC = null;
            float closestDistance = DetectionRange;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && npc.CanBeChasedBy())
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNPC = npc;
                    }
                }
            }

            return closestNPC;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Visual impact effect
            for (int i = 0; i < 8; i++)
            {
                Color dustColor = Main.rand.Next(2) switch
                {
                    0 => new Color(255, 200, 100),
                    _ => new Color(200, 200, 200)
                };

                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Copper, 0f, 0f, 100, dustColor, 1.2f);
                dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
                dust.noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Bounce off tiles
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.7f;
            }

            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.7f;
            }

            return false;
        }
    }
}