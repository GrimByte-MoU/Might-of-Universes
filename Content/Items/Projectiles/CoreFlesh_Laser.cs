using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common; // ReaperDamageClass

namespace MightofUniverses.Content.Items.Projectiles
{
    // Fast piercing laser, pierces once (2 hits total), no souls on hit
    public class CoreFlesh_Laser : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 80;
            Projectile.penetrate = 2; 
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 2;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0.9f, 0.2f, 0.9f) * 0.4f);

            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, 0f, 0f, 160, new Color(255, 120, 120), 0.9f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.2f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 8; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, 0f, 0f, 160, new Color(255, 160, 100), 1.1f);
                Main.dust[d].noGravity = true;
            }
            return base.OnTileCollide(oldVelocity);
        }
    }
}