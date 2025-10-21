using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class YakusokuPetal : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 810;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.075f;

            if (Main.rand.NextBool(4))
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    new Vector2(Main.rand.NextFloat(-2f, 2f), 2f),
                    ModContent.ProjectileType<SmallYakusokuPetal>(),
                    (int)(Projectile.damage * 0.6f),
                    Projectile.knockBack,
                    Projectile.owner
                );
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, 1.0f, 0.6f, 0.8f);
        }
    }
}
