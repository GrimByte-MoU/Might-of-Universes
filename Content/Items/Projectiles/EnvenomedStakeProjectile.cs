using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class EnvenomedStakeProjectile : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Envenomed Stake");
        }

        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 1; // Arrow-like movement
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 2; // Pierce 2 enemies
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            // Create a trail of venom particles
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.Venom, 0f, 0f, 100, default, 1f);
                dust.noGravity = true;
                dust.scale = 1f + Main.rand.NextFloat() * 0.5f;
                dust.velocity *= 0.3f;
            }
            
            // Rotate the projectile based on velocity
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Apply Acid Venom debuff (Venom buff in vanilla)
            target.AddBuff(BuffID.Venom, 300); // 5 seconds of Acid Venom
            
            // Create a burst of venom particles on hit
            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Venom, speed * 3f, Scale: 1.5f);
                dust.noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Create venom splash effect
            for (int i = 0; i < 15; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Venom, speed * 2f, Scale: 1.2f);
                dust.noGravity = true;
            }
            
            // Play splash sound
            SoundEngine.PlaySound(SoundID.Item54, Projectile.position);
            
            return true; // Destroy the projectile
        }
    }
}
