using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CrossboneProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Projectile.damage = 15;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
        }
            public override void AI()
    {
        //Projectile.rotation += 0.3f;
    }
    }
}

