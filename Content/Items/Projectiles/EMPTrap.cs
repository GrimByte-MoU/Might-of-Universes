using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class EMPTrap : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 100;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void AI()
        {
            Projectile.alpha = 100 + (int)(Math.Sin(Projectile.ai[0] * 0.1f) * 50);
            Projectile.ai[0]++;

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Electric,
                    0f,
                    0f,
                    100,
                    Color.Cyan,
                    1.5f
                );
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }

            Lighting.AddLight(Projectile.Center, 0f, 1f, 1f);

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.Hitbox.Intersects(Projectile.Hitbox))
                {
                    Explode();
                    return;
                }
            }
        }

        private void Explode()
        {
            for (int i = 0; i < 100; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.Center - new Vector2(150, 150),
                    300,
                    300,
                    DustID.Electric,
                    0f,
                    0f,
                    100,
                    Color.Cyan,
                    3f
                );
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(10f, 10f);
            }

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            float explosionRadius = 300f;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && Vector2.Distance(npc.Center, Projectile.Center) <= explosionRadius)
                {
                    npc.StrikeNPC(new NPC.HitInfo
                    {
                        Damage = Projectile.damage,
                        Knockback = 0f,
                        HitDirection = npc.Center.X > Projectile.Center.X ? 1 : -1
                    });

                    if (!npc.boss)
                    {
                        npc.AddBuff(ModContent.BuffType<Paralyze>(), 180);
                    }
                }
            }

            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position, 
                    Projectile.width, 
                    Projectile.height, 
                    DustID.Electric, 
                    0f, 
                    0f, 
                    100, 
                    Color.Cyan, 
                    1.5f
                );
                dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                dust.noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 255, 255, 100);
        }
    }
}