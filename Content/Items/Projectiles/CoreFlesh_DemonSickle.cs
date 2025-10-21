using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common; // ReaperDamageClass

namespace MightofUniverses.Content.Items.Projectiles
{
    // Spins in place for ~0.5s then launches in the original aim direction. Very high damage. Ignores up to 50 defense.
    public class CoreFlesh_DemonSickle : MoUProjectile
    {
        private const int SpinTime = 30; // ticks
        private const float LaunchSpeed = 14f;

        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 3;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.ArmorPenetration = 50;
        }

        public override void AI()
        {
            int timer = (int)Projectile.ai[1];
            if (timer < SpinTime)
            {
                // Spin in place
                Projectile.velocity *= 0.92f;
                Projectile.rotation += 0.45f * Projectile.direction;
                if (Projectile.velocity.LengthSquared() < 0.01f)
                    Projectile.velocity = Vector2.Zero;

                if (Main.rand.NextBool(3))
                {
                    int d = Dust.NewDust(Projectile.Center - new Vector2(4, 4), 8, 8, DustID.DemonTorch, 0f, 0f, 150, new Color(180, 50, 255), 1.2f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0.2f;
                }

                timer++;
                Projectile.ai[1] = timer;
            }
            else if (timer == SpinTime)
            {
                // Launch
                float ang = Projectile.ai[0];
                Vector2 dir = ang.ToRotationVector2();
                Projectile.velocity = dir * LaunchSpeed;
                Projectile.netUpdate = true;

                for (int i = 0; i < 10; i++)
                {
                    int d = Dust.NewDust(Projectile.Center - new Vector2(6, 6), 12, 12, DustID.PurpleTorch, dir.X * 1.2f, dir.Y * 1.2f, 150, new Color(180, 50, 255), 1.1f);
                    Main.dust[d].noGravity = true;
                }

                Projectile.ai[1] = timer + 1;
            }
            else
            {
                // Flight
                Projectile.rotation += 0.25f * Projectile.direction;
                Lighting.AddLight(Projectile.Center, new Vector3(0.6f, 0.2f, 0.8f) * 0.5f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X * 0.4f;
            if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y * 0.4f;
            Projectile.penetrate--;
            return Projectile.penetrate <= 0;
        }
    }
}