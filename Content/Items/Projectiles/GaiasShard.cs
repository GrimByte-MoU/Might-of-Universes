using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class GaiasShard : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = 0;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            // Small sparkle trail (optional)
            if (Main.rand.NextBool(4))
            {
                int d = Dust.NewDust(Projectile.Center, 1, 1, DustID.GreenTorch, 0f, 0f, 0, default, 1f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.3f;
            }
            if (Projectile.ai[0] < 10f)
            {
                Projectile.velocity *= 0.85f;
                Projectile.ai[0]++;
                return;
            }
            NPC target = FindClosestTarget(Projectile.Center, 260f);
            if (target != null)
            {
                float accel = 0.20f;
                float maxSpeed = 11f;
                Vector2 desired = Projectile.DirectionTo(target.Center) * maxSpeed;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired, accel);
            }
            else
            {
                Projectile.velocity *= 0.99f;
            }
            Lighting.AddLight(Projectile.Center, 0f, 2f, 0f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Shards are fragile; break on tiles
            Projectile.Kill();
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 1 second of Terra's Rend
            target.AddBuff(ModContent.BuffType<ElementsHarmony>(), 60);
        }

        private NPC FindClosestTarget(Vector2 from, float maxRange)
        {
            NPC closest = null;
            float bestSq = maxRange * maxRange;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (n.CanBeChasedBy())
                {
                    float d = Vector2.DistanceSquared(from, n.Center);
                    if (d < bestSq && Collision.CanHitLine(Projectile.Center, 1, 1, n.Center, 1, 1))
                    {
                        bestSq = d;
                        closest = n;
                    }
                }
            }
            return closest;
        }
    }
}
