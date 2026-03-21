using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class EchoShard : MoUProjectile
    {
        private const float MAX_SPEED = 24f;
        private const float ACCELERATION = 0.4f;
        private const float HOME_RANGE = 600f;

        public override void SafeSetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.alpha = 80;
            Projectile.light = 0.6f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

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
                    Projectile.velocity *= 1.03f;
                }
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.RainbowMk2,
                    0f,
                    0f,
                    100,
                    default,
                    1.2f
                );
                dust.noGravity = true;
                dust.velocity *= 0.2f;
            }

            Lighting.AddLight(Projectile.Center, 0.5f, 0.3f, 0.7f);
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
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(1f, target.Center);

            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    DustID.RainbowMk2,
                    Main.rand.NextFloat(-3f, 3f),
                    Main.rand.NextFloat(-3f, 3f),
                    100,
                    default,
                    1.8f
                );
                dust.noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(180, 120, 240, 120);
        }
    }
}