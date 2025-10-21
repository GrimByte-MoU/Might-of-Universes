using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TempleReaperFireball : MoUProjectile
    {
        private int bounceCount = 0;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 5; // Number of bounces
            Projectile.light = 0.8f; // Bright fiery light
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            // Emit torch dust particles
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default(Color), 1.5f);

            // Rotate the projectile
            //Projectile.rotation += 0.1f * (float)Projectile.direction;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(3f, target.Center);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bounceCount++;
            if (bounceCount >= 5)
            {
                Projectile.Kill(); // Destroy the projectile after 5 bounces
                return true;
            }

            // Reverse velocity on collision
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }

            return false; // Do not destroy the projectile on collision
        }

        public override void Kill(int timeLeft)
        {
            // Create explosion effect on death
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 100, default(Color), 2f);
            }
        }
    }
}
