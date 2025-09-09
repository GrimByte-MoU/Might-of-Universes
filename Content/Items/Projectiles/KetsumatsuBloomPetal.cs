using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class KetsumatsuBloomPetal : ModProjectile
    {
        public override void SetDefaults()
    {
        Projectile.width = 12;
        Projectile.height = 12;
        Projectile.friendly = true;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 180;
        Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
    }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, 1.0f, 0.6f, 0.8f); // Light pink glow
        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PinkCrystalShard);
    }
    }
}
