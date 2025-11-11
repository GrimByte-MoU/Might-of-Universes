using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CelestialReaperUltimate : MoUProjectile
    {
        private List<int> hitNPCs = new List<int>();
        private int currentTarget = -1;
        private const int LIFETIME = 300;
        private int searchCooldown = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = LIFETIME;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.scale = 1.5f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Fade in at start, fade out at end
            if (Projectile.timeLeft > LIFETIME - 30)
            {
                Projectile.alpha = (int)(255 * (1f - ((LIFETIME - Projectile.timeLeft) / 30f)));
            }
            else if (Projectile.timeLeft < 30)
            {
                Projectile.alpha = (int)(255 * (1f - (Projectile.timeLeft / 30f)));
            }
            else
            {
                Projectile.alpha = 0;
            }

            // Search for next target
            searchCooldown--;
            if (searchCooldown <= 0)
            {
                searchCooldown = 5; // Search every 5 frames
                currentTarget = FindNextScreenEnemy(player);
            }

            // Move to current target or die if none found
            if (currentTarget >= 0 && Main.npc[currentTarget].active && !Main.npc[currentTarget].dontTakeDamage)
            {
                NPC target = Main.npc[currentTarget];
                Vector2 toTarget = target.Center - Projectile.Center;
                float distance = toTarget.Length();

                if (distance > 50f)
                {
                    toTarget.Normalize();
                    Projectile.velocity = toTarget * 25f;
                }
                else
                {
                    // Close enough, mark as hit and find next
                    if (!hitNPCs.Contains(currentTarget))
                    {
                        hitNPCs.Add(currentTarget);
                    }
                    currentTarget = -1;
                    searchCooldown = 0; // Search immediately
                }
            }
            else
            {
                // No valid target - check if any enemies remain on screen
                if (FindNextScreenEnemy(player) < 0)
                {
                    // No enemies left on screen - disappear
                    Projectile.Kill();
                    return;
                }
            }

            Projectile.rotation += 0.5f;

            // Epic rainbow trail
            if (Main.rand.NextBool())
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowMk2, 0f, 0f, 100, default, 2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.5f;
            }
        }

        private int FindNextScreenEnemy(Player player)
        {
            Rectangle screenRect = new Rectangle(
                (int)(player.Center.X - Main.screenWidth / 2),
                (int)(player.Center.Y - Main.screenHeight / 2),
                Main.screenWidth,
                Main.screenHeight
            );

            float closestDist = float.MaxValue;
            int closestIndex = -1;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                
                // Skip invalid targets
                if (!npc.active || npc.friendly || npc.lifeMax <= 5 || npc.dontTakeDamage || !npc.CanBeChasedBy())
                    continue;

                // Only target enemies on screen
                if (!screenRect.Contains(npc.Center.ToPoint()))
                    continue;

                // Prefer enemies we haven't hit yet
                if (hitNPCs.Contains(i))
                    continue;

                float dist = Vector2.Distance(Projectile.Center, npc.Center);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestIndex = i;
                }
            }

            // If no unhit enemies, allow re-hitting
            if (closestIndex < 0)
            {
                hitNPCs.Clear(); // Reset hit list to allow bouncing again
                
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.active || npc.friendly || npc.lifeMax <= 5 || npc.dontTakeDamage || !npc.CanBeChasedBy())
                        continue;

                    if (!screenRect.Contains(npc.Center.ToPoint()))
                        continue;

                    float dist = Vector2.Distance(Projectile.Center, npc.Center);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closestIndex = i;
                    }
                }
            }

            return closestIndex;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Visual impact effect
            SoundEngine.PlaySound(SoundID.Item62 with { Volume = 0.7f, Pitch = 0.3f }, target.Center);

            for (int i = 0; i < 15; i++)
            {
                Vector2 dustVel = Main.rand.NextVector2Circular(6f, 6f);
                int dust = Dust.NewDust(target.Center, 4, 4, DustID.RainbowMk2, dustVel.X, dustVel.Y, 100, default, 2f);
                Main.dust[dust].noGravity = true;
            }
            target.AddBuff(BuffID.Daybreak, 180);
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = texture.Size() * 0.5f;

            // Rainbow trail
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float alpha = 1f - (i / (float)Projectile.oldPos.Length);
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                
                Color rainbowColor = Main.hslToRgb((Main.GameUpdateCount * 0.01f + i * 0.1f) % 1f, 1f, 0.5f);
                rainbowColor *= alpha * 0.7f * ((255 - Projectile.alpha) / 255f);

                Main.EntitySpriteDraw(texture, drawPos, null, rainbowColor, Projectile.rotation, drawOrigin, Projectile.scale * 0.9f, SpriteEffects.None, 0);
            }

            // Glow
            Color glowColor = Color.White * ((255 - Projectile.alpha) / 255f) * 0.3f;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, glowColor, Projectile.rotation, drawOrigin, Projectile.scale * 1.3f, SpriteEffects.None, 0);

            // Main draw
            Color mainColor = Color.White * ((255 - Projectile.alpha) / 255f);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, mainColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}