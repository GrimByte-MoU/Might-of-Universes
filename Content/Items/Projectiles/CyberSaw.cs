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
    public class CyberSaw : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        private const float MAX_SPEED = 25f;
        private const float HOME_RANGE = 1000f;
        private const float ACCELERATION = 0.5f;

        public override void AI()
        {
            Projectile.rotation += 0.4f;

            if (Main.rand.NextBool(2))
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
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile.Center, 0f, 1f, 1f);

            NPC target = FindClosestEnemy();
            if (target != null)
            {
                Vector2 direction = target.Center - Projectile.Center;
                direction.Normalize();
                
                Projectile.velocity += direction * ACCELERATION;
                
                if (Projectile.velocity.Length() > MAX_SPEED)
                {
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= MAX_SPEED;
                }
            }
            else
            {
                if (Projectile.velocity.Length() < MAX_SPEED)
                {
                    Projectile.velocity *= 1.05f;
                }
            }
        }

        private NPC FindClosestEnemy()
        {
            NPC closestNPC = null;
            float closestDistance = HOME_RANGE;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.CanBeChasedBy())
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNPC = npc;
                    }
                }
            }

            return closestNPC;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<TerrasRend>(), 180);

            for (int i = 0; i < 25; i++)
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
                    2.5f
                );
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(6f, 6f);
            }

            SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
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
                    1.8f
                );
                dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
                dust.noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(100, 255, 255, 200);
        }
    }
}