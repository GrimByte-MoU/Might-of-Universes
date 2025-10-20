using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class BloodShard : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 24;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.aiStyle = -1;
            Projectile.scale = 0.3f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            int lifeSteal = (int)(damageDone * 0.05f);
            player.statLife += lifeSteal;
            player.Heal(lifeSteal);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Shatter, Projectile.position);

            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Glass, 
                    Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 0, default, 1f);
            }
        }

        public override void AI()
        {
            float targetRotation = Projectile.velocity.ToRotation();
            //Projectile.rotation = targetRotation;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
{
    modifiers.DefenseEffectiveness *= 0.7f; // Reduces defense effectiveness by 30%
}

    }
}
