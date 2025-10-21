using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class KiokuMedium : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 45;
            Projectile.ignoreWater = true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Vector2 vel = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(60 * i)) * 5f;
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    vel,
                    ModContent.ProjectileType<KiokuSmall>(),
                    (int)(Projectile.damage * 0.8f),
                    Projectile.knockBack,
                    Projectile.owner
                );
            }
        }
        public override void AI() {
        Lighting.AddLight(Projectile.Center, 1.0f, 0.6f, 0.8f);
        }
    }
}
