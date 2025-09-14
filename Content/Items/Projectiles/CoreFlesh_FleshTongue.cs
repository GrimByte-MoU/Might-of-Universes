using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent; // TextureAssets

namespace MightofUniverses.Content.Items.Projectiles
{
    // True beam: draws a dense pixel line, hits the first NPC along the line, does not pierce, grants 3 souls.
    public class CoreFlesh_FleshTongue : ModProjectile
    {
        private const float MaxRange = 480f;         // decent range (~30 tiles)
        private const float Step = 6f;               // step size for tile scan
        private const float BeamThickness = 10f;     // render width and hit tolerance
        private const int Lifetime = 12;             // ticks; beam lingers very briefly after hit
        private const int SoulOnHit = 3;

        // ai[0] = aim rotation (radians)
        // localAI[0] = 0 not yet damaged; 1 already dealt damage

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = Lifetime;
            Projectile.penetrate = -1; // we manually control hit; "non-piercing" by logic
            Projectile.tileCollide = false; // beam ignores tile by logic (we stop at first tile)
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 0;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Projectile.hide = true; // drawn via PreDraw
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10; // irrelevant since we only hit once
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active)
            {
                Projectile.Kill();
                return;
            }
            Projectile.Center = owner.MountedCenter;
            if (Projectile.localAI[0] == 0f)
            {
                TryDealBeamHit(owner);
                Projectile.localAI[0] = 1f;
            }

            // Add subtle light
            Lighting.AddLight(Projectile.Center, new Vector3(1.0f, 0.15f, 0.15f) * 0.35f);
        }

        private void TryDealBeamHit(Player owner)
        {
            // Only do hit logic on server/single-player to avoid double hits in MP
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            Vector2 start = owner.MountedCenter;
            Vector2 dir = Projectile.ai[0].ToRotationVector2().SafeNormalize(Vector2.UnitX);

            // Find beam end (first solid tile or max range)
            Vector2 end = ScanFirstSolid(start, dir, MaxRange);

            // Find the first NPC intersected along the line
            NPC first = null;
            float bestT = float.MaxValue;
            foreach (NPC npc in Main.npc)
            {
                if (npc == null || !npc.active || npc.friendly || npc.lifeMax <= 1)
                    continue;

                // Project NPC center onto line and measure distance to segment
                float t = ClosestPointOnSegmentT(start, end, npc.Center);
                if (t < 0f || t > 1f) continue;

                Vector2 onLine = Vector2.Lerp(start, end, t);
                float dist = Vector2.Distance(npc.Center, onLine);

                // Hit if within half thickness + a small padding
                if (dist <= BeamThickness * 0.5f + Math.Max(6f, npc.width * 0.1f))
                {
                    if (t < bestT)
                    {
                        bestT = t;
                        first = npc;
                    }
                }
            }

            if (first != null)
            {
                int direction = Math.Sign(dir.X);
                int damage = Projectile.damage;       // scaled damage passed in at spawn
                float knockback = Projectile.knockBack;

                // Apply damage and small knockback
                var hit = new NPC.HitInfo
                {
                    Damage = damage,
                    Knockback = knockback,
                    HitDirection = direction,
                    Crit = false
                };
                first.StrikeNPC(hit);

                // Souls on hit
                owner.GetModPlayer<ReaperPlayer>().AddSoulEnergy(SoulOnHit, first.Center);

                // Shrink remaining lifetime so the beam fades quickly after hitting
                Projectile.timeLeft = Math.Min(Projectile.timeLeft, 6);
            }
        }

        private static Vector2 ScanFirstSolid(Vector2 start, Vector2 dir, float maxRange)
        {
            // Step forward until hitting a solid tile or exceeding max range
            float traveled = 0f;
            while (traveled < maxRange)
            {
                Vector2 p = start + dir * traveled;
                // Sample a small box around the line point
                if (Collision.SolidCollision(p - new Vector2(2f, 2f), 4, 4))
                {
                    // Stop slightly before the solid point for better visuals
                    return start + dir * Math.Max(0f, traveled - 4f);
                }
                traveled += Step;
            }
            return start + dir * maxRange;
        }

        private static float ClosestPointOnSegmentT(Vector2 a, Vector2 b, Vector2 p)
        {
            Vector2 ab = b - a;
            float abLenSq = ab.LengthSquared();
            if (abLenSq <= 1e-6f) return 0f;
            float t = Vector2.Dot(p - a, ab) / abLenSq;
            if (t < 0f) return 0f;
            if (t > 1f) return 1f;
            return t;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active) return false;

            Vector2 start = owner.MountedCenter;
            Vector2 dir = Projectile.ai[0].ToRotationVector2().SafeNormalize(Vector2.UnitX);
            Vector2 end = ScanFirstSolid(start, dir, MaxRange);

            // Draw the beam as a thick rectangle strip using MagicPixel
            Texture2D px = TextureAssets.MagicPixel.Value;
            Vector2 origin = new Vector2(0.5f, 0.5f);

            float length = Vector2.Distance(start, end);
            if (length <= 1f) return false;

            float rot = (end - start).ToRotation();

            // Color gradient head->tail
            Color head = new Color(255, 60, 60, 230);
            Color tail = new Color(160, 20, 20, 100);

            // Slight pulsation and taper over lifetime
            float lifeT = 1f - Projectile.timeLeft / (float)Lifetime;
            float thickness = BeamThickness * MathHelper.Lerp(1f, 0.8f, lifeT);

            // Draw a few layered passes for a denser look
            for (int i = 0; i < 3; i++)
            {
                float layerT = i / 2f; // 0, 0.5, 1
                Color c = Color.Lerp(head, tail, layerT) * (0.8f - 0.25f * i);
                float layerThick = thickness * (1.0f - 0.2f * i);

                Main.EntitySpriteDraw(
                    px,
                    (start + end) * 0.5f - Main.screenPosition,
                    null,
                    c,
                    rot,
                    origin,
                    new Vector2(length, layerThick),
                    SpriteEffects.None,
                    0f
                );
            }

            return false; // we've drawn it
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(0.6f, target.Center);
        }
    }
}