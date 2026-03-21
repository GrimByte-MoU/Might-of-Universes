using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class EchoSoulsaw : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 180;
            Projectile.alpha = 30;
            Projectile.light = 1.2f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
        }

        public override void AI()
        {
            Projectile.rotation += 0.6f;

            for (int i = 0; i < 3; i++)
            {
                float angle = Projectile.rotation + (i * MathHelper.TwoPi / 3f);
                Vector2 offset = angle.ToRotationVector2() * 30f;
                
                Dust dust = Dust.NewDustPerfect(
                    Projectile.Center + offset,
                    DustID.RainbowMk2,
                    Vector2.Zero,
                    100,
                    default,
                    2.5f
                );
                dust.noGravity = true;
                dust.velocity = -Projectile.velocity * 0.2f;
            }

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.RainbowMk2,
                    0f,
                    0f,
                    100,
                    default,
                    2f
                );
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile.Center, 1f, 0.7f, 1.2f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    DustID.RainbowMk2,
                    Main.rand.NextFloat(-8f, 8f),
                    Main.rand.NextFloat(-8f, 8f),
                    100,
                    default,
                    Main.rand.NextFloat(2.5f, 4f)
                );
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 40; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.RainbowMk2,
                    Main.rand.NextFloat(-10f, 10f),
                    Main.rand.NextFloat(-10f, 10f),
                    100,
                    default,
                    3f
                );
                dust.noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 200, 255, 200);
        }
    }
}