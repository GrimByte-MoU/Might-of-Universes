using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ViralOrb : MoUProjectile
    {
        private int fragmentCooldown = 0;
        private int timeStationary = 0;
        private bool hasSlowed = false;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.light = 0.3f;
            Projectile.scale = 2f;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Red.ToVector3() * 0.6f);

            if (!hasSlowed)
            {
                Projectile.velocity *= 0.98f;
                if (Projectile.velocity.Length() < 0.2f)
                {
                    hasSlowed = true;
                    Projectile.velocity = Vector2.Zero;
                }
            }
            else
            {
                timeStationary++;
                fragmentCooldown++;
                if (fragmentCooldown >= 30)
                {
                    FireRadialFragments();
                    fragmentCooldown = 0;
                }

                if (timeStationary >= 120)
                {
                    FinalBurst();
                    Projectile.Kill();
                }
            }
        }

        private void FireRadialFragments()
        {
            for (int i = 0; i < 8; i++)
            {
                float angle = MathHelper.TwoPi * i / 8f;
                Vector2 velocity = angle.ToRotationVector2() * 5f;
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    velocity,
                    ModContent.ProjectileType<ViralFragment>(),
                    (int)(Projectile.damage * 0.35f),
                    Projectile.knockBack,
                    Projectile.owner
                );
            }
        }

        private void FinalBurst()
        {
            for (int i = 0; i < 16; i++)
            {
                float angle = MathHelper.TwoPi * i / 16f;
                Vector2 velocity = angle.ToRotationVector2() * 6f;
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    velocity,
                    ModContent.ProjectileType<ViralFragment>(),
                    (int)(Projectile.damage * 0.35f),
                    Projectile.knockBack,
                    Projectile.owner
                );
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DefenseEffectiveness *= 0.5f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Buffs.CodeDestabilized>(), 600);
        }
    }
}
