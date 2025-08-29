using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CodeBlastProjectile : ModProjectile
    {
        public NPC targetNPC;
        private const float HOMING_STRENGTH = 0.3f;

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.light = 0.8f;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (targetNPC != null && targetNPC.active)
            {
                Vector2 toTarget = targetNPC.Center - Projectile.Center;
                toTarget.Normalize();
                Projectile.velocity = (Projectile.velocity * (1f - HOMING_STRENGTH)) + (toTarget * HOMING_STRENGTH * 20f);
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, 0f, 1f, 0f);
            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.GreenTorch, 0f, 0f, 100, default, 1.5f);
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}
