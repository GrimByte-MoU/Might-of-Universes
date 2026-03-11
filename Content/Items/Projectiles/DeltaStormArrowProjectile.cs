using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class DeltaStormArrowProjectile : ModProjectile
    {
        private bool isMaxSpeed = false;
        private int accelerationTimer = 0;
        private const int MaxSpeedTime = 180;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 600;
            Projectile.arrow = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            accelerationTimer++;

            if (accelerationTimer >= MaxSpeedTime && !isMaxSpeed)
            {
                isMaxSpeed = true;
                
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
                    dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                }
            }

            float maxSpeed = 24f;
            float minSpeed = 3f;
            float targetSpeed = MathHelper.Lerp(minSpeed, maxSpeed, accelerationTimer / (float)MaxSpeedTime);
            
            Vector2 direction = Projectile.velocity;
            direction.Normalize();
            Projectile.velocity = direction * targetSpeed;

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
                dust.noGravity = true;
                dust.scale = isMaxSpeed ? 1.2f : 0.8f;
            }

            Lighting.AddLight(Projectile.Center, 0.3f, 0.6f, 1f);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (isMaxSpeed)
            {
                modifiers.SetCrit();
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DeltaShock>(), 180);
        }
    }
}