using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;

namespace MightofUniverses.Content.Items.Projectiles
{
    // Invisible, near-instant golden zap that chains up to 3 times (4 hits total).
    // Slim visuals, robust line collision to prevent misses.
    public class HorusZapProjectile : ModProjectile
    {
        private static readonly float[] ChainMultipliers = new float[] { 1f, 0.85f, 0.60f, 0.45f };

        private const float DefaultChainRangePx = 30f * 16f;
        private const float TravelSpeed = 100f;
        private const int ExtraUpdates = 10;

        private const float BeamVisualThicknessPx = 1.2f; // visual dust spread
        private const float BeamHitboxThicknessPx = 6f;   // collision thickness (slightly thicker than before)

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.extraUpdates = ExtraUpdates;
            Projectile.DamageType = DamageClass.Summon;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.alpha = 255;
            Projectile.hide = true;
        }

        public override bool PreDraw(ref Color lightColor) => false;

        public override void AI()
        {
            if (Projectile.localAI[1] == 0f)
            {
                if (Projectile.velocity.LengthSquared() > 0.001f)
                {
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= TravelSpeed;
                }
                Projectile.localAI[1] = 1f;
            }

            // Trail across full-tick path
            Vector2 end = Projectile.Center;
            Vector2 start = end - Projectile.velocity; // full-tick segment
            SpawnBeamDustAlong(start, end, BeamVisualThicknessPx);

            // Warm gold light
            Lighting.AddLight(Projectile.Center, 0.2f, 0.16f, 0.04f);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 end = Projectile.Center;
            Vector2 start = end - Projectile.velocity; // full-tick path
            float _ = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, BeamHitboxThicknessPx, ref _))
                return true;

            return null;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int step = (int)Projectile.ai[0];
            int lastHitId = (int)Projectile.ai[1];

            int nextStep = step + 1;
            if (nextStep >= ChainMultipliers.Length)
                return;

            float chainRangePx = Projectile.localAI[0] > 0 ? Projectile.localAI[0] : DefaultChainRangePx;

            NPC next = FindNextTarget(target.Center, chainRangePx, avoidA: target.whoAmI, avoidB: lastHitId);
            if (next == null && lastHitId >= 0 && lastHitId < Main.maxNPCs)
            {
                NPC prev = Main.npc[lastHitId];
                if (prev.CanBeChasedBy(this) && Vector2.DistanceSquared(target.Center, prev.Center) <= chainRangePx * chainRangePx)
                    next = prev;
            }

            if (next != null)
            {
                DrawZapBetween(target.Center, next.Center, BeamVisualThicknessPx);

                int baseDamage = System.Math.Max(1, Projectile.originalDamage > 0 ? Projectile.originalDamage : Projectile.damage);
                int nextDamage = System.Math.Max(1, (int)System.Math.Round(baseDamage * ChainMultipliers[nextStep]));

                Vector2 dir = next.Center - target.Center;
                if (dir.LengthSquared() < 1f) dir = new Vector2(0f, -1f);
                dir.Normalize();

                int idx = Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    target.Center,
                    dir * TravelSpeed,
                    Projectile.type,
                    nextDamage,
                    0f,
                    Projectile.owner,
                    ai0: nextStep,
                    ai1: target.whoAmI
                );

                if (idx >= 0 && idx < Main.maxProjectiles)
                {
                    Main.projectile[idx].originalDamage = baseDamage;
                    Main.projectile[idx].localAI[0] = chainRangePx;
                }
            }
        }

        private NPC FindNextTarget(Vector2 from, float rangePx, int avoidA, int avoidB)
        {
            NPC chosen = null;
            float best = rangePx * rangePx;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.CanBeChasedBy(this)) continue;
                if (npc.whoAmI == avoidA || npc.whoAmI == avoidB) continue;

                float d2 = Vector2.DistanceSquared(from, npc.Center);
                if (d2 <= best)
                {
                    best = d2;
                    chosen = npc;
                }
            }
            return chosen;
        }

        private void SpawnBeamDustAlong(Vector2 a, Vector2 b, float thicknessPx)
        {
            Vector2 dir = b - a;
            float len = dir.Length();
            if (len < 0.1f) return;
            dir /= len;
            Vector2 perp = new Vector2(-dir.Y, dir.X);

            int points = System.Math.Max(4, (int)(len / 6f));
            for (int i = 0; i <= points; i++)
            {
                float t = i / (float)points;
                Vector2 p = Vector2.Lerp(a, b, t);

                float off = Main.rand.NextFloat(-thicknessPx, thicknessPx);
                Vector2 pos = p + perp * off;

                int dustType = Main.rand.NextBool(3) ? DustID.Sand : DustID.GoldFlame;
                var d = Dust.NewDustDirect(pos - new Vector2(1, 1), 2, 2, dustType, 0f, 0f, 80, default, Main.rand.NextFloat(0.9f, 1.2f));
                d.noGravity = true;
                d.velocity *= 0f;
            }
        }

        private void DrawZapBetween(Vector2 a, Vector2 b, float thicknessPx)
        {
            Vector2 dir = b - a;
            float len = dir.Length();
            if (len < 1f) return;
            dir /= len;
            Vector2 perp = new Vector2(-dir.Y, dir.X);

            int points = System.Math.Max(10, (int)(len / 8f));
            for (int i = 0; i <= points; i++)
            {
                float t = i / (float)points;
                Vector2 p = Vector2.Lerp(a, b, t);

                float off = Main.rand.NextFloat(-thicknessPx, thicknessPx);
                Vector2 pos = p + perp * off;

                int dustType = Main.rand.NextBool(3) ? DustID.Sand : DustID.GoldFlame;
                var d = Dust.NewDustDirect(pos - new Vector2(1, 1), 2, 2, dustType, 0f, 0f, 70, default, Main.rand.NextFloat(1.0f, 1.3f));
                d.noGravity = true;
                d.velocity *= 0f;
            }
        }
    }
}