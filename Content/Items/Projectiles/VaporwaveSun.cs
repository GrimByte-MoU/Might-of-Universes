using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class VaporwaveSun : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20; // Hit same enemy every ~0.33 seconds
        }

        public override bool? CanHitNPC(NPC target) => true;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            
            // Kill if player doesn't have the set
            if (!player.GetModPlayer<Common.Players.VaporParadisePlayer>().hasVaporParadiseSet)
            {
                Projectile.Kill();
                return;
            }

            var vaporPlayer = player.GetModPlayer<Common.Players.VaporParadisePlayer>();
            
            // Determine if this is an ability sun (ai[2] == 1) or base sun (ai[2] == 0)
            bool isAbilitySun = Projectile.ai[2] == 1f;
            
            // Get radius and speed based on ability state
            float radius;
            float rotationSpeed;
            int totalSunCount;
            
            if (vaporPlayer.abilityActive)
            {
                // During ability: all suns orbit far and fast
                radius = 25 * 16;
                rotationSpeed = 0.08f;
                Projectile.damage = 90;
                totalSunCount = 8;
            }
            else
            {
                // Normal state: only base suns orbit close and slow
                if (isAbilitySun)
                {
                    // Ability suns should be killed when ability ends
                    // (handled in VaporParadisePlayer)
                    return;
                }
                radius = 15 * 16;
                rotationSpeed = 0.04f;
                Projectile.damage = 45;
                totalSunCount = 4;
            }
            
            // Use ai[0] as the fixed position index
            // Base suns: 0, 1, 2, 3
            // Extra suns: 0.5, 1.5, 2.5, 3.5
            float sunPosition = Projectile.ai[0];
            
            // Orbit calculation based on fixed position
            float rotation = sunPosition * MathHelper.TwoPi / 4f; // Always divide by 4 to get even spacing
            rotation += Projectile.ai[1]; // Current rotation angle
            
            Vector2 offset = new Vector2(
                (float)Math.Cos(rotation) * radius,
                (float)Math.Sin(rotation) * radius
            );
            
            Projectile.Center = player.Center + offset;
            
            // Increment rotation
            Projectile.ai[1] += rotationSpeed;
            
            // Visual effects - more intense for ability suns
            float dustChance = isAbilitySun ? 2f : 3f;
            if (Main.rand.NextBool((int)dustChance))
            {
                Color dustColor = Main.rand.Next(3) switch
                {
                    0 => new Color(255, 100, 255), // Hot pink
                    1 => new Color(100, 255, 255), // Cyan
                    _ => new Color(200, 100, 255)  // Purple
                };
                
                float scale = isAbilitySun ? 1.5f : 1.2f;
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.RainbowMk2, Vector2.Zero, 100, dustColor, scale);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
            }
            
            // Sun glow - brighter for ability suns
            float lightIntensity = isAbilitySun ? 1.5f : 1f;
            Lighting.AddLight(Projectile.Center, 1f * lightIntensity, 0.6f * lightIntensity, 1f * lightIntensity);
        }
    }
}