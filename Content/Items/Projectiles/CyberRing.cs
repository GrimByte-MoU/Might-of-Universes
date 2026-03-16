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
    public class CyberRing : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Projectile.damage = 150;
        }

        private bool hasCyberOneSet = false;
        private const float MAX_SPEED = 20f;
        private const float HOME_RANGE = 800f;
        private const float ACCELERATION = 0.3f;

        public override void AI()
        {
            hasCyberOneSet = Projectile.ai[0] == 1f;

            Projectile.rotation += 0.3f;

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
                    1.2f
                );
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }

            Lighting.AddLight(Projectile.Center, 0f, 0.8f, 0.8f);

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
            target.AddBuff(BuffID.Electrified, 300);

            if (hasCyberOneSet && !target.boss)
            {
                target.AddBuff(ModContent.BuffType<Paralyze>(), 18);
            }

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
                    2f
                );
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(5f, 5f);
            }

            SoundEngine.PlaySound(SoundID.Item94, Projectile.Center);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
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
            return new Color(100, 255, 255, 200);
        }
    }
}