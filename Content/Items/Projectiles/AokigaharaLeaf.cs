using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class AokigaharaLeaf : MoUProjectile
    {
        private const float HOME_RANGE = 800f;
        private const float HOME_STRENGTH = 0.4f;
        private const float MAX_SPEED = 20f;

        public override void SafeSetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Projectile.alpha = 50;
            Projectile.light = 0.8f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI()
        {
            Projectile.rotation += 0.15f * Math.Sign(Projectile.velocity.X);

            NPC target = FindClosestEnemy();
            if (target != null)
            {
                Vector2 direction = target.Center - Projectile.Center;
                direction.Normalize();
                
                Projectile.velocity += direction * HOME_STRENGTH;
                
                if (Projectile.velocity.Length() > MAX_SPEED)
                {
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= MAX_SPEED;
                }
            }

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.PinkFairy,
                    0f,
                    0f,
                    100,
                    Color.Fuchsia,
                    1.2f
                );
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            float pulse = (float)Math.Sin(Main.GameUpdateCount * 0.1f) * 0.3f + 0.7f;
            Lighting.AddLight(Projectile.Center, 1f * pulse, 0f, 1f * pulse);
        }

        private NPC FindClosestEnemy()
        {
            NPC closestNPC = null;
            float closestDistance = HOME_RANGE;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.CanBeChasedBy() && !npc.dontTakeDamage)
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
            reaper.AddSoulEnergy(3f, target.Center);

            target.AddBuff(ModContent.BuffType<NaturesToxin>(), 90);

            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    DustID.PinkFairy,
                    Main.rand.NextFloat(-5f, 5f),
                    Main.rand.NextFloat(-5f, 5f),
                    100,
                    Color.Fuchsia,
                    1.8f
                );
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.PinkFairy,
                    Main.rand.NextFloat(-4f, 4f),
                    Main.rand.NextFloat(-4f, 4f),
                    100,
                    Color.Fuchsia,
                    2f
                );
                dust.noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 0, 255, 150);
        }
    }
}