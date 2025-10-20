using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class SanguineDroplet : ModProjectile
    {
        private int timeElapsed = 0;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            timeElapsed++;
            float targetRotation = Projectile.velocity.ToRotation();
            //Projectile.rotation = targetRotation;
            
            if (timeElapsed > 30)
            {
                Projectile.velocity.Y += 0.4f;
            }

            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 0, default, 1f);
        }
    }
}
