using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class LaserSaberBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.EnchantedBeam);
            
            Projectile.penetrate = 12;
            Projectile.ArmorPenetration = 50;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            base.AI();

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, Color.Cyan, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile.Center, 0.3f, 1f, 1f);
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Cyan;
        }
    }
}