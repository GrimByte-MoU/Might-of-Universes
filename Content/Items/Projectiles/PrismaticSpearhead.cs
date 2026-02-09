using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Weapons;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class PrismaticSpearhead : MoUProjectile
    {
        private int colorTimer = 0;
        private readonly Color[] prismaticColors = new Color[]
        {
            new Color(255, 0, 0),
            new Color(255, 127, 0),
            new Color(255, 255, 0),
            new Color(0, 255, 0),
            new Color(0, 0, 255),
            new Color(148, 0, 211)
        };

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 9;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.5f);
            Projectile.rotation = Projectile.velocity.ToRotation();
            
            Projectile.velocity *= 0.99f;

            colorTimer++;
            if (colorTimer >= 30)
            {
                colorTimer = 0;
                ShootColoredSpearheads();
            }

            if (Main.rand.NextBool(2))
            {
                Color dustColor = prismaticColors[Main.rand.Next(prismaticColors.Length)];
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.RainbowTorch, 0f, 0f, 100, dustColor, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
        }

        private void ShootColoredSpearheads()
        {
            int colorIndex = Projectile.timeLeft / 10 % 6;
            if (colorIndex >= 0 && colorIndex < prismaticColors.Length)
            {
                Vector2 velocity = Projectile.velocity * 1f;
                float angle = (float)Math.PI / 3;
                Vector2 velocityAbove = velocity.RotatedBy(angle);
                Vector2 velocityBelow = velocity.RotatedBy(-angle);

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    velocityAbove,
                    ModContent.ProjectileType<ColoredSpearhead>(),
                    (int)(Projectile.damage * 0.5f),
                    Projectile.knockBack * 0.5f,
                    Projectile.owner,
                    ai0: colorIndex
                );

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    velocityBelow,
                    ModContent.ProjectileType<ColoredSpearhead>(),
                    (int)(Projectile.damage * 0.5f),
                    Projectile.knockBack * 0.5f,
                    Projectile.owner,
                    ai0: colorIndex
                );
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
        }
    }
}
