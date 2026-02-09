using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class NatureStakeProjectile : MoUProjectile
    {
        private int bounceCount = 0;
        private const int MAX_BOUNCES = 3;

        public override void SetStaticDefaults()
        {
        }

        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.GrassBlades, 0f, 0f, 100, default, 1f);
                dust.noGravity = true;
                dust.scale = 1f + Main.rand.NextFloat() * 0.5f;
                dust.velocity *= 0.3f;
            }
            
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 15; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.GrassBlades, speed * 3f, Scale: 1.5f);
                dust.noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bounceCount++;
            if (bounceCount <= MAX_BOUNCES)
            {
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
                
                if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                {
                    Projectile.velocity.X = -oldVelocity.X * 0.9f;
                }
                
                if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.9f;
                }
                
                for (int i = 0; i < 10; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.GrassBlades, speed * 2f, Scale: 1.2f);
                    dust.noGravity = true;
                }
                
                return false;
            }
            
            return true;
        }
    }
}
