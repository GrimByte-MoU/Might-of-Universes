using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;


namespace MightofUniverses.Content.Items.Projectiles
{
    public class CactusThorn : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = 1;
            Projectile.scale = 0.5f;
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
float rotation = Projectile.velocity.ToRotation();
Projectile.rotation = rotation;

            
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.t_Cactus);
            }
            Projectile.spriteDirection = Projectile.direction;

        }
    }
}
