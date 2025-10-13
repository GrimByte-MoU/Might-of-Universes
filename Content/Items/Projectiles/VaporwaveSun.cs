using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class VaporwaveSun : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
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
            
            // Get radius and speed based on ability state
            float radius = vaporPlayer.abilityActive ? 25 * 16 : 15 * 16;
            float rotationSpeed = vaporPlayer.abilityActive ? 0.08f : 0.04f;
            
            // Update damage based on ability
            Projectile.damage = vaporPlayer.abilityActive ? 90 : 45;
            
            // Orbit calculation
            float rotation = Projectile.ai[0] * MathHelper.TwoPi / 4f; // 4 suns evenly spaced
            rotation += Projectile.ai[1]; // Current rotation angle
            
            Vector2 offset = new Vector2(
                (float)Math.Cos(rotation) * radius,
                (float)Math.Sin(rotation) * radius
            );
            
            Projectile.Center = player.Center + offset;
            
            // Increment rotation
            Projectile.ai[1] += rotationSpeed;
            
            // Visual effects - vaporwave aesthetic (pink/cyan/purple)
            if (Main.rand.NextBool(3))
            {
                Color dustColor = Main.rand.Next(3) switch
                {
                    0 => new Color(255, 100, 255), // Hot pink
                    1 => new Color(100, 255, 255), // Cyan
                    _ => new Color(200, 100, 255)  // Purple
                };
                
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.RainbowMk2, Vector2.Zero, 100, dustColor, 1.2f);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
            }
            
            // Sun glow
            Lighting.AddLight(Projectile.Center, 1f, 0.6f, 1f); // Purple-pink glow
        }
    }
}