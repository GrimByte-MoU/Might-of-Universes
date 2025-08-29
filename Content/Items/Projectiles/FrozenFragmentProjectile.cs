using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class FrozenFragmentProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 30;
            Projectile.light = 0.3f;
            Projectile.alpha = 50;
            Projectile.scale = 0.8f;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.3f, 0.8f);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                DustID.IceRod, 0f, 0f, 100, default, 0.8f);
            
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity *= 0.98f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 180);
        }
    }
}
