using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class SoundFractureBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.PulseBolt);
            
            Projectile.penetrate = 11;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        private int bounceCount = 0;
        private const int MaxBounces = 11;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (bounceCount < MaxBounces)
            {
                bounceCount++;
                
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
                
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
                
                for (int i = 0; i < 5; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, Color.Cyan, 1.0f);
                    dust.noGravity = true;
                    dust.velocity = Main.rand.NextVector2Circular(2f, 2f);
                }
                
                return false;
            }
            
            return true;
        }

        public override void AI()
        {
            base.AI();

            Lighting.AddLight(Projectile.Center, 0.3f, 1f, 1f);
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}