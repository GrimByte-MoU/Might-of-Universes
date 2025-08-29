using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class MushroomSporeStrong : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.2f, 0.2f, 0.5f); 
            Projectile.velocity *= 0.95f;

            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GlowingMushroom, 0f, 0f, 150, default, 1.2f);
        }
    }
}
