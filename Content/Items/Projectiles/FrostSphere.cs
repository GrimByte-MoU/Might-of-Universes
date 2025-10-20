using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class FrostSphere : ModProjectile
    {
        private int timer = 0;

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 14;
            Projectile.timeLeft = 300;
            Projectile.light = 1f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.1f, 0.6f, 1f);
            Projectile.spriteDirection = 1;
            //Projectile.rotation += 0.2f;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.IceRod, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
            }

            timer++;
            if (timer >= 30)
            {
                timer = 0;
                ShootIceSpikes();
            }

            Projectile.velocity *= 1.01f;
        }

        private void ShootIceSpikes()
        {
            for (int i = 0; i < 6; i++)
            {
                Vector2 velocity = Vector2.One.RotatedBy(MathHelper.ToRadians(60 * i)) * 8f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity,
                    ModContent.ProjectileType<IceSpike>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 300);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Shatter, Projectile.position);
            for (int i = 0; i < 30; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceRod);
            }
        }
    }
}

