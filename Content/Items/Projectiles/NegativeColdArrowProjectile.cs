using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class NegativeColdArrowProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 600;
            Projectile.arrow = true;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice);
                dust.noGravity = true;
                dust.scale = 0.9f;
            }

            Lighting.AddLight(Projectile.Center, 0.2f, 0.4f, 0.8f);
            Projectile.rotation = Projectile.velocity.ToRotation();

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            bool hadSheerCold = target.HasBuff(ModContent.BuffType<SheerCold>());
            
            target.AddBuff(ModContent.BuffType<SheerCold>(), 180);

            if (hadSheerCold && !target.boss && Main.rand.NextFloat() < 0.33f)
            {
                target.AddBuff(BuffID.Confused, 180);
                
                for (int i = 0; i < 15; i++)
                {
                    Dust dust = Dust.NewDustDirect(target.Center - new Vector2(10, 10), 20, 20, DustID.Ice);
                    dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                    dust.noGravity = true;
                    dust.scale = 1.3f;
                }
            }
        }
    }
}