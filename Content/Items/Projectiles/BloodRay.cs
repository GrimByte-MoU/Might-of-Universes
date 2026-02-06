using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class BloodRay : MoUProjectile
    {
        private bool initialized = false;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;
            Projectile.aiStyle = -1;
            Projectile.scale = 1.0f;
        }

        public override void AI()
        {
            if (!initialized)
            {
                initialized = true;
                Projectile.velocity *= 0.8f;
            }

            Lighting.AddLight(Projectile.Center, 1f, 0.1f, 0.1f);
            
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.Blood, 0f, 0f, 100, default, 1.2f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(target.position, target.width, target.height, DustID.Blood,
                    Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1.2f);
            }
            SoundEngine.PlaySound(SoundID.NPCHit13, target.position);
            target.AddBuff(ModContent.BuffType<EnemyBleeding>(), 180);
        }
    }
}

