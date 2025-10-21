using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class VolatileSandShard : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Projectile.tileCollide = true;
            Projectile.timeLeft = 90;
        }

        public override void AI()
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Sand);
        }
    }
}