using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class WaferSwordSprinkle : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 24;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.aiStyle = -1;
            Projectile.scale = 1f;
        }
        public override void Kill(int timeLeft)
        {

            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedStarfish, 
                    Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 0, default, 1f);
            }
        }

        public override void AI()
        {
            float targetRotation = Projectile.velocity.ToRotation();
            //Projectile.rotation = targetRotation;
        }
    }
}