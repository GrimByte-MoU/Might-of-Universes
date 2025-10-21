using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class UnmeiPetal : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 10;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.velocity.X += (float)System.Math.Sin(Projectile.timeLeft * 0.3f) * 0.25f;
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PinkCrystalShard);
            Lighting.AddLight(Projectile.Center, 1.0f, 0.6f, 0.8f);
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}
