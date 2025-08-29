using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TerraiumLightning : ModProjectile
    {
        private List<Vector2> lightningPath;
        private int currentSegment;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 60;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 60;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            int targetIndex = (int)Projectile.ai[0];
            int delay = (int)Projectile.ai[1];

            // Wait before striking
            if (Projectile.localAI[0]++ < delay)
                return;

            if (lightningPath == null)
            {
                lightningPath = new List<Vector2>();
                currentSegment = 0;

                if (targetIndex >= 0 && targetIndex < Main.maxNPCs && Main.npc[targetIndex].active)
                {
                    NPC target = Main.npc[targetIndex];
                    Vector2 strikeStart = target.Center + new Vector2(0, -600f);
                    Vector2 strikeEnd = target.Center;

                    int segments = 15;
                    for (int i = 0; i <= segments; i++)
                    {
                        float t = i / (float)segments;
                        Vector2 pos = Vector2.Lerp(strikeStart, strikeEnd, t);
                        float jaggedness = 40f * (float)(1.0 - Math.Abs(t - 0.5) * 2.0); // More jagged in the middle
                        pos.X += Main.rand.NextFloat(-jaggedness, jaggedness);

                        lightningPath.Add(pos);
                    }
                }
                else
                {
                    Projectile.Kill();
                    return;
                }
            }

            if (currentSegment < lightningPath.Count)
            {
                Projectile.Center = lightningPath[currentSegment];

                // Spawn dust along the segment
                Dust.NewDustPerfect(Projectile.Center, DustID.Electric, Vector2.Zero, 150, Color.LightBlue, 1.5f).noGravity = true;
                currentSegment++;
            }
            else
            {
                // Strike at the end
                NPC target = Main.npc[targetIndex];
                if (target != null && target.active)
                {
                    var hitInfo = new NPC.HitInfo()
                    {
                        Damage = Projectile.damage,
                        Knockback = 0f,
                        HitDirection = 0,
                        Crit = false
                    };
                    target.StrikeNPC(hitInfo);
                }
                Projectile.Kill();
            }
        }
    }
}
