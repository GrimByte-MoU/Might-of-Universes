using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class MegaBusterChargeLevel1 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 300;
            Projectile.light = 1.0f;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            
            float scale = Projectile.ai[0];
            if (scale > 0)
            {
                Projectile.scale = scale;
            }
            
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, Color.Cyan, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }
            
            Lighting.AddLight(Projectile.Center, 0.3f, 1f, 1f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Cyan;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Electric, 0f, 0f, 100, Color.Cyan, 1.8f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
            }
        }
    }
}