using System;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class AetherSerpentBolt : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 1;
            Projectile.scale = 0.75f;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.25f, 0.25f, 0.7f);
            Projectile.rotation = Projectile.velocity.ToRotation();
            int target = FindTarget(Projectile.Center, 550f);
            if (target != -1)
            {
                NPC npc = Main.npc[target];
                float speed = Math.Clamp(Projectile.velocity.Length(), 7f, 14f);
                Vector2 desired = (npc.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * speed;
                float inertia = 18f;
                Projectile.velocity = (Projectile.velocity * (inertia - 1f) + desired) / inertia;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<RebukingLight>(), 60);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X * 0.5f;
            if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y * 0.5f;
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0) Projectile.Kill();
            return false;
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