using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class DragonFlameDust : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.96f;
            
            Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(10));

            for (int i = 0; i < 2; i++)
            {
                Vector2 randomOffset = Main.rand.NextVector2Circular(6f, 6f);
                Vector2 dustPos = Projectile.Center + randomOffset;
                
                float scale = Main.rand.NextFloat(1.0f, 2.5f);
                if (Projectile.timeLeft < 10)
                    scale *= Projectile.timeLeft / 10f;
                
                int dustIndex = Dust.NewDust(dustPos, 1, 1, DustID.Torch, 0f, 0f, 100, Color.Red, scale);
                Dust dust = Main.dust[dustIndex];
                dust.noGravity = true;
                dust.velocity = Projectile.velocity * 0.5f + randomOffset * 0.15f;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= 0.75f;
            modifiers.ArmorPenetration += 50;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CoreHeat>(), 180);
        }
    }
}