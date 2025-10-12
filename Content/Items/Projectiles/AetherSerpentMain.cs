using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class AetherSerpentMain : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 240;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1.25f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, 0.35f, 0.35f, 0.9f);
            if (Projectile.localAI[0] < 30f)

            {
                Projectile.velocity *= 0.98f;
                Projectile.localAI[0]++;
            }
            else
            {
                Projectile.velocity *= 1.01f;
            }
            int target = FindTarget(Projectile.Center, 600f);
            if (target != -1)
            {
                NPC npc = Main.npc[target];
                float speed = MathHelper.Clamp(Projectile.velocity.Length(), 6f, 16f);
                Vector2 desired = (npc.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * speed;
                float inertia = 20f;
                Projectile.velocity = (Projectile.velocity * (inertia - 1f) + desired) / inertia;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<RebukingLight>(), 180);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner != Main.myPlayer) return;

            int bolts = 7;
            for (int i = 0; i < bolts; i++)
            {
                Vector2 v = (Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi)) * Main.rand.NextFloat(7f, 10f);
                int dmg = (int)(Projectile.damage * 0.6f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, v,
                    ModContent.ProjectileType<AetherSerpentBolt>(), dmg, Projectile.knockBack * 0.8f, Projectile.owner);
            }

            // Burst dust
            for (int d = 0; d < 10; d++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.HallowedTorch, Scale: 1.1f);
                Main.dust[dust].velocity *= 1.8f;
                Main.dust[dust].noGravity = true;
            }
        }

        private int FindTarget(Vector2 from, float maxRange)
        {
            int best = -1;
            float bestDist = maxRange;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (!n.active || n.friendly || n.dontTakeDamage || !n.CanBeChasedBy()) continue;
                float d = Vector2.Distance(from, n.Center);
                if (d < bestDist && Collision.CanHitLine(from, 1, 1, n.Center, 1, 1))
                {
                    best = i;
                    bestDist = d;
                }
            }
            return best;
        }
    }
}