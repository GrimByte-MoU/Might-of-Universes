using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class MoonWave : MoUProjectile
    {
        private float curveDirection = 1f;
        private const float CURVE_STRENGTH = 0.2f;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 30;
            Projectile.light = 0.5f;
            Projectile.alpha = 100;
            Projectile.scale = 1.15f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            curveDirection = Projectile.ai[1];
            if (Projectile.timeLeft < 8)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(CURVE_STRENGTH * curveDirection);
            }
            Lighting.AddLight(Projectile.Center, 0.2f, 0.2f, 1f);
            
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.LunarOre,
                    0f,
                    0f,
                    100,
                    default,
                    1f
                );
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<LunarReap>(), 120);
        }
    }
}