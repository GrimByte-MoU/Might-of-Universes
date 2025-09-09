using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;


namespace MightofUniverses.Content.Items.Projectiles
{
    public class CactusThorn : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
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
            
            // Makes projectile face its movement direction
float rotation = Projectile.velocity.ToRotation();
Projectile.rotation = rotation;

// Optional: Add this if you want the sprite to be oriented differently
// Projectile.rotation += MathHelper.PiOver2; // Rotates sprite 90 degrees

            
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.t_Cactus);
            }
            Projectile.spriteDirection = Projectile.direction;

        }
    }
}
