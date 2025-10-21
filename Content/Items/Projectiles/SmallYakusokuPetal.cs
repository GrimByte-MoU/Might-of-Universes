using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class SmallYakusokuPetal : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.1f;
            Projectile.position.X += (float)System.Math.Sin(Projectile.timeLeft / 10f) * 0.8f;
            Projectile.rotation = Projectile.velocity.ToRotation();
        Lighting.AddLight(Projectile.Center, 1.0f, 0.6f, 0.8f);
        
        }
    }
}
