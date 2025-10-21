using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class WardenSpearProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.light = 1f;
        }

        public override void AI()
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Water);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch);
            Lighting.AddLight(Projectile.Center, 0f, 0f, 1.2f);
        }
    }
}
