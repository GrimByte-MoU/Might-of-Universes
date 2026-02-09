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
            Projectile.localNPCHitCooldown = 20;
        }

        public override bool? CanHitNPC(NPC target) => true;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.GetModPlayer<VaporParadisePlayer>().hasVaporParadiseSet)
            {
                Projectile.Kill();
                return;
            }

            var vaporPlayer = player.GetModPlayer<VaporParadisePlayer>();
            bool isAbilitySun = Projectile.ai[2] == 1f;
            float radius;
            float rotationSpeed;
            int totalSunCount;
            
            if (vaporPlayer.abilityActive)
            {
                radius = 25 * 16;
                rotationSpeed = 0.08f;
                Projectile.damage = 90;
                totalSunCount = 8;
            }
            else
            {
                if (isAbilitySun)
                {
                    return;
                }
                radius = 15 * 16;
                rotationSpeed = 0.04f;
                Projectile.damage = 45;
                totalSunCount = 4;
            }
            
            float sunPosition = Projectile.ai[0];
            float rotation = sunPosition * MathHelper.TwoPi / 4f;
            rotation += Projectile.ai[1];
            
            Vector2 offset = new Vector2(
                (float)Math.Cos(rotation) * radius,
                (float)Math.Sin(rotation) * radius
            );
            
            Projectile.Center = player.Center + offset;
            Projectile.ai[1] += rotationSpeed;
            float dustChance = isAbilitySun ? 2f : 3f;
            if (Main.rand.NextBool((int)dustChance))
            {
                Color dustColor = Main.rand.Next(3) switch
                {
                    0 => new Color(255, 100, 255),
                    1 => new Color(100, 255, 255),
                    _ => new Color(200, 100, 255)
                };
                
                float scale = isAbilitySun ? 1.5f : 1.2f;
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.RainbowMk2, Vector2.Zero, 100, dustColor, scale);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
            }

            float lightIntensity = isAbilitySun ? 1.5f : 1f;
            Lighting.AddLight(Projectile.Center, 1f * lightIntensity, 0.6f * lightIntensity, 1f * lightIntensity);
        }
    }
}