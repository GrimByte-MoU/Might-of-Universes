using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class IceSaw : ModProjectile
    {
        private int sphereCount = 0;
        private int chargeTimer = 0;
        private const int MAX_SPHERES = 11;
        private const int SPAWN_RATE = 15;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
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

            if (!player.channel || !player.active || player.dead)
            {
                ReleaseAllSaws(player);
                return;
            }

            Projectile.timeLeft = 2;
            Projectile.Center = player.Center;

            chargeTimer++;
            if (chargeTimer >= SPAWN_RATE && sphereCount < MAX_SPHERES)
            {
                chargeTimer = 0;
                sphereCount++;
                
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<OrbitingIceSaw>(),
                        Projectile.damage,
                        Projectile.knockBack,
                        Projectile.owner,
                        sphereCount,
                        Projectile.whoAmI
                    );
                }

                SoundEngine.PlaySound(SoundID.Item28, player.Center);
            }

            if (sphereCount >= MAX_SPHERES)
            {
                ReleaseAllSaws(player);
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Ice, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }
        }

        private void ReleaseAllSaws(Player player)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == ModContent.ProjectileType<OrbitingIceSaw>() && proj.ai[1] == Projectile.whoAmI)
                {
                    proj.ai[0] = -1;
                }
            }

            Projectile.Kill();
        }

        public override bool? CanDamage() => false;
    }
}