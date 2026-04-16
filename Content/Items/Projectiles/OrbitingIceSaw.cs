using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class OrbitingIceSaw : MoUProjectile
    {
        private bool launched = false;
        private int totalSaws = 0;

        private ref float SawIndex => ref Projectile.ai[0];
        private ref float ParentWhoAmI => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (SawIndex < 0)
            {
                if (!launched)
                {
                    LaunchTowardsCursor(player);
                }
                return;
            }

            Projectile.timeLeft = 2;
            
            float orbitRadius = 100f;
            float orbitSpeed = 0.05f;
            float baseAngle = (SawIndex - 1) * MathHelper.TwoPi / 10f;
            float angle = baseAngle + (Main.GameUpdateCount * orbitSpeed);

            Vector2 orbitPosition = player.Center + new Vector2(
                (float)Math.Cos(angle) * orbitRadius,
                (float)Math.Sin(angle) * orbitRadius
            );

            Projectile.Center = orbitPosition;
            Projectile.rotation += 0.3f;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, 0f, 0f, 100, default, 1f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
        }

        private void LaunchTowardsCursor(Player player)
        {
            launched = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 180;

            totalSaws = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == Projectile.type && proj.ai[1] == ParentWhoAmI)
                {
                    totalSaws++;
                }
            }

            Vector2 targetPosition = Main.MouseWorld;
            Vector2 direction = targetPosition - Projectile.Center;
            direction.Normalize();
            
            Projectile.velocity = direction * 18f;

            SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            float damageMultiplier = 1f + (totalSaws * 0.1f);
            modifiers.SourceDamage *= damageMultiplier;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SheerCold>(), 180);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
            
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, 0f, 0f, 100, default, 2f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
            }
        }

        public override bool? CanDamage() => launched;
    }
}