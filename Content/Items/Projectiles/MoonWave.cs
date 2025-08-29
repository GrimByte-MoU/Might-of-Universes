using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class MoonWave : ModProjectile
    {
        private float curveDirection = 1f;
        private const float CURVE_STRENGTH = 0.1f;

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 60;
            Projectile.light = 0.5f;
            Projectile.alpha = 100;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            curveDirection = Projectile.ai[1];
            Projectile.velocity = Projectile.velocity.RotatedBy(CURVE_STRENGTH * curveDirection);
            
            // Electric effects
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