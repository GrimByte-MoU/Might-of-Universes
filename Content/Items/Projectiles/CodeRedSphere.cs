using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CodeRedSphere : ModProjectile
    {
        private const float RADIUS = 10f * 16f;
        private const int BASE_DAMAGE = 750;
        private const int DAMAGE_PER_ENEMY = 75;
        private const float HOMING_SPEED = 0.3f;
        private const float HOVER_DISTANCE = 50f;
        private int damageTimer;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.alpha = 100;
        }

        public override void AI()
        {
            DrawCircleEffect();
            HandleHoming();
            HandleDamage();
        }

        private void DrawCircleEffect()
        {
            const int NUM_POINTS = 42;
            for (int i = 0; i < NUM_POINTS; i++)
            {
                float angle = (float)(i * (2 * Math.PI) / NUM_POINTS);
                Vector2 offset = new Vector2(
                    (float)Math.Cos(angle) * RADIUS,
                    (float)Math.Sin(angle) * RADIUS
                );
                Vector2 dustPos = Projectile.Center + offset;
                
                Dust dust = Dust.NewDustPerfect(
                    dustPos,
                    DustID.SparksMech,
                    Vector2.Zero,
                    0,
                    Color.White,
                    1f
                );
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        private void HandleHoming()
        {
            float maxDetectRadius = 400f;
            NPC targetNPC = FindClosestNPC(maxDetectRadius);

            if (targetNPC != null)
            {
                Vector2 desiredPosition = targetNPC.Center - new Vector2(0, HOVER_DISTANCE);
                Vector2 moveDirection = desiredPosition - Projectile.Center;
                float distance = moveDirection.Length();

                if (distance > 5f)
                {
                    moveDirection.Normalize();
                    Projectile.velocity += moveDirection * HOMING_SPEED;
                    Projectile.velocity *= 0.98f;
                }
                else
                {
                    Projectile.velocity *= 0.95f;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        private NPC FindClosestNPC(float maxDetectRadius)
        {
            NPC closestNPC = null;
            float closestDistance = maxDetectRadius;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy())
                {
                    float distance = Vector2.Distance(npc.Center, Projectile.Center);
                    if (distance < closestDistance)
                    {
                        closestNPC = npc;
                        closestDistance = distance;
                    }
                }
            }

            return closestNPC;
        }

        private void HandleDamage()
        {
            damageTimer++;
            if (damageTimer >= 6) // Increased to 10 times per second (60 / 6 = 10)
            {
                damageTimer = 0;
                List<NPC> affectedNPCs = new List<NPC>();

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && Vector2.Distance(npc.Center, Projectile.Center) <= RADIUS)
                    {
                        affectedNPCs.Add(npc);
                    }
                }

                if (affectedNPCs.Count > 0)
                {
                    int damagePerNPC = BASE_DAMAGE + (affectedNPCs.Count * DAMAGE_PER_ENEMY);
                    foreach (NPC npc in affectedNPCs)
                    {
                        npc.StrikeNPC(new NPC.HitInfo
                        {
                            Damage = damagePerNPC,
                            Knockback = 0f,
                            HitDirection = 0
                        });

                        CreateHitEffect(npc);
                    }
                }
            }
        }

        private void CreateHitEffect(NPC npc)
        {
            for (int d = 0; d < 3; d++)
            {
                Dust.NewDust(
                    npc.position,
                    npc.width,
                    npc.height,
                    DustID.LifeDrain,
                    0f,
                    0f,
                    100,
                    default,
                    1f
                );
            }
        }
    }
}
