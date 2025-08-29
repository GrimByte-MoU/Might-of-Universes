using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ArcaneGladiusProjectile : ModProjectile
    {
        private const int SpiralFrames = 36;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] < SpiralFrames)
            {
                float angle = Projectile.ai[0] * 0.3f + Projectile.whoAmI;
                Vector2 spiralOffset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 2.5f;
                Projectile.velocity = (player.DirectionTo(Projectile.Center) * 2f) + spiralOffset;
                Projectile.ai[0]++;
            }
            else
            {
                Vector2 destination = Main.MouseWorld;
                Vector2 toCursor = destination - Projectile.Center;
                float desiredSpeed = 14f;
                if (toCursor.Length() > 32f)
                {
                    toCursor.Normalize();
                    Vector2 desiredVelocity = toCursor * desiredSpeed;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.13f);
                }
                else
                {
                    Projectile.velocity = toCursor;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}
