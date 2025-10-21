using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class DeglitchingSphereRanged : MoUProjectile
    {
        private const float RADIUS = 7f * 16f;
        private const int BASE_DAMAGE = 60;
        private const int DAMAGE_PER_ENEMY = 6;
        private int damageTimer;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.alpha = 100;
        }

        public override void AI()
        {
            // Draw the circle
            const int NUM_POINTS = 42; // Number of points in the circle
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
                    DustID.BlueTorch,
                    Vector2.Zero,
                    0,
                    Color.White,
                    1f
                );
                dust.noGravity = true;
                dust.noLight = true;
            }

            // Handle damage every 12 ticks (5 times per second)
            damageTimer++;
            if (damageTimer >= 8)
            {
                damageTimer = 0;
                List<NPC> affectedNPCs = new List<NPC>();

                // Find all NPCs in range
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && Vector2.Distance(npc.Center, Projectile.Center) <= RADIUS)
                    {
                        affectedNPCs.Add(npc);
                    }
                }

                // Calculate and apply damage
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

                        // Create hit effect
                        for (int d = 0; d < 3; d++)
                        {
                            Dust.NewDust(
                                npc.position,
                                npc.width,
                                npc.height,
                                DustID.Electric,
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
        }
    }
}