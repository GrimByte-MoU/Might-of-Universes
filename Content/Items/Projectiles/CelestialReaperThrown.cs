using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CelestialReaperThrown : MoUProjectile
    {
        private bool wasFullyCharged = false;
        private bool returning = false;
        private int radialBurstTimer = 0;
        private const int RADIAL_BURST_RATE = 10;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Kill if player is dead or gone
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // First frame: check if this scythe was thrown at full charge
            if (Projectile.localAI[0] == 0f)
            {
                wasFullyCharged = (Projectile.ai[0] == 1f);
                Projectile.localAI[0] = 1f;
            }

            Projectile.rotation += 0.4f;

            // Radial bursts happen THE ENTIRE TIME if fully charged (not just during outward flight)
            if (wasFullyCharged)
            {
                radialBurstTimer++;
                if (radialBurstTimer >= RADIAL_BURST_RATE)
                {
                    radialBurstTimer = 0;
                    SpawnRadialBurst();
                }
            }

            if (!returning)
            {
                // Flying out phase
                Projectile.velocity *= 0.97f; // Slow down more gradually

                // Start returning after travelling far enough OR velocity gets too slow
                float distanceFromPlayer = Vector2.Distance(Projectile.Center, player.Center);
                if (Projectile.velocity.Length() < 3f || distanceFromPlayer > 800f)
                {
                    returning = true;
                    SoundEngine.PlaySound(SoundID.Item7, Projectile.position); // Boomerang return sound
                }
            }
            else
            {
                // Returning phase - boomerang back to player
                Vector2 toPlayer = player.Center - Projectile.Center;
                float distance = toPlayer.Length();

                // Catch the scythe when close enough
                if (distance < 60f)
                {
                    Projectile.Kill();
                    SoundEngine.PlaySound(SoundID.Item1, player.Center); // Catch sound
                    return;
                }

                // Accelerate toward player
                toPlayer.Normalize();
                float baseReturnSpeed = 18f;
                float speedBoost = Math.Max(0f, (800f - distance) * 0.02f); // Speed up as it gets closer
                float returnSpeed = baseReturnSpeed + speedBoost;
                
                // Smooth acceleration
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, toPlayer * returnSpeed, 0.08f);
            }

            // Rainbow trail dust
            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowMk2, 0f, 0f, 100, default, 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
            }
        }

        private void SpawnRadialBurst()
        {
            SoundEngine.PlaySound(SoundID.Item9 with { Volume = 0.6f, Pitch = 0.2f }, Projectile.position);

            // Spawn 16 orbs in a perfect circle
            for (int i = 0; i < 16; i++)
            {
                float angle = (MathHelper.TwoPi / 16f) * i;
                Vector2 velocity = angle.ToRotationVector2() * 10f;

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity,
                    ModContent.ProjectileType<CelestialOrb>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack * 0.5f, Projectile.owner);
            }

            // Visual burst effect
            for (int i = 0; i < 20; i++)
            {
                Vector2 dustVel = Main.rand.NextVector2Circular(8f, 8f);
                int dust = Dust.NewDust(Projectile.Center, 4, 4, DustID.RainbowMk2, dustVel.X, dustVel.Y, 100, default, 1.8f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            ReaperPlayer reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.AddSoulEnergy(2f, target.Center);
            target.AddBuff(BuffID.Daybreak, 180);
        }

        public override bool SafePreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawOrigin = texture.Size() * 0.5f;

            // Draw rainbow trail
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float alpha = 1f - (i / (float)Projectile.oldPos.Length);
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                Color color = Color.Cyan * alpha * 0.5f;
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * 0.8f, SpriteEffects.None, 0);
            }

            // Draw glow if was fully charged
            if (wasFullyCharged)
            {
                Color glowColor = new Color(100, 200, 255, 0) * 0.4f;
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, glowColor, Projectile.rotation, drawOrigin, Projectile.scale * 1.2f, SpriteEffects.None, 0);
            }

            // Draw main projectile
            Vector2 mainDrawPos = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(texture, mainDrawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}