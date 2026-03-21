using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class EchoBlade : MoUProjectile
    {
        private const float ORBIT_RADIUS = 120f;
        private const float ROTATION_SPEED = 0.1f;
        private const int ORBIT_TIME = 180;
        private bool hasLaunched = false;

        public override void SafeSetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 240;
            Projectile.alpha = 50;
            Projectile.light = 1f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            int bladeIndex = (int)Projectile.ai[0];
            Projectile.ai[1]++;

            if (Projectile.ai[1] < ORBIT_TIME && !hasLaunched)
            {
                float baseAngle = bladeIndex / 10f * MathHelper.TwoPi;
                float angle = baseAngle + (Projectile.ai[1] * ROTATION_SPEED);

                Vector2 offset = new Vector2(
                    ORBIT_RADIUS * (float)Math.Cos(angle),
                    ORBIT_RADIUS * (float)Math.Sin(angle)
                );

                Projectile.Center = player.Center + offset;
                Projectile.rotation = angle + MathHelper.PiOver4;

                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.RainbowMk2,
                        0f,
                        0f,
                        100,
                        default,
                        1.5f
                    );
                    dust.noGravity = true;
                    dust.velocity *= 0.3f;
                }
            }
            else if (!hasLaunched)
            {
                hasLaunched = true;
                NPC target = FindClosestEnemy();
                
                if (target != null)
                {
                    Vector2 direction = target.Center - Projectile.Center;
                    direction.Normalize();
                    Projectile.velocity = direction * 25f;
                }
                else
                {
                    Projectile.velocity = new Vector2(player.direction * 20f, 0f);
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
                
                NPC target = FindClosestEnemy();
                if (target != null)
                {
                    Vector2 direction = target.Center - Projectile.Center;
                    direction.Normalize();
                    Projectile.velocity += direction * 0.6f;
                    
                    if (Projectile.velocity.Length() > 30f)
                    {
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 30f;
                    }
                }
            }

            Lighting.AddLight(Projectile.Center, 0.8f, 0.5f, 1f);
        }

        private NPC FindClosestEnemy()
        {
            NPC closestNPC = null;
            float closestDistance = 1000f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.CanBeChasedBy())
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
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    DustID.RainbowMk2,
                    Main.rand.NextFloat(-6f, 6f),
                    Main.rand.NextFloat(-6f, 6f),
                    100,
                    default,
                    Main.rand.NextFloat(2f, 3f)
                );
                dust.noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(220, 180, 255, 180);
        }
    }
}