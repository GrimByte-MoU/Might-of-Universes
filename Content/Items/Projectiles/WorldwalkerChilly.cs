using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{

    public class WorldwalkerChilly : MoUProjectile
    {
        // ====== CONFIG (Primary Projectile) ======
        private const int IcicleCount = 6;
        private const float DamagePerIcicleFrac = 1f;
        private const float FanTotalWidth = 220f;
        private const float SpawnHeightAbove = 420f;
        private const float InitialDownSpeed = 1.2f;
        private const float RandomHorizontalJitter = 18f;
        private const bool SpawnOnTileHit = false; // Set true to also spawn volley on tile collision
        // ==========================================

        private bool spawnedVolley;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(6))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice,
                    Projectile.velocity.X * 0.18f, Projectile.velocity.Y * 0.18f, 140, default, 0.85f);
                Main.dust[d].noGravity = true;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 300);
            TrySpawnVolley(target);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (SpawnOnTileHit)
            {
            }
            Projectile.Kill();
            return false;
        }

        private void TrySpawnVolley(NPC target)
        {
            if (spawnedVolley || Main.myPlayer != Projectile.owner)
                return;

            spawnedVolley = true;

            int npcIndex = (target != null) ? target.whoAmI : -1;
            Vector2 anchor = (target != null) ? target.Center : Projectile.Center;
            float baseDamage = Projectile.damage;

            for (int i = 0; i < IcicleCount; i++)
            {
                float t = (IcicleCount == 1) ? 0.5f : i / (float)(IcicleCount - 1);
                float offsetX = MathHelper.Lerp(-FanTotalWidth * 0.5f, FanTotalWidth * 0.5f, t)
                                + Main.rand.NextFloat(-RandomHorizontalJitter, RandomHorizontalJitter);

                Vector2 spawnPos = new(anchor.X + offsetX, anchor.Y - SpawnHeightAbove);

                float damageFrac = DamagePerIcicleFrac;

                int icicleDamage = (int)(baseDamage * damageFrac);

                // ai0 = npcIndex, ai1 = isLast (1 / 0)
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    spawnPos,
                    new Vector2(0f, InitialDownSpeed),
                    ModContent.ProjectileType<ChillyIcicle>(),
                    icicleDamage,
                    Projectile.knockBack * 0.65f,
                    Projectile.owner,
                    ai0: npcIndex
                );
            }

            // Spawn burst dust
            for (int i = 0; i < 14; i++)
            {
                Vector2 v = Main.rand.NextVector2Circular(3.4f, 3.4f);
                int d = Dust.NewDust(anchor, 0, 0, DustID.SnowBlock, v.X, v.Y, 150, default, 1.15f);
                Main.dust[d].noGravity = true;
            }

            Projectile.Kill();
        }
    }

    public class ChillyIcicle : MoUProjectile
    {
        // ====== CONFIG (Homing Icicles) ======
        private const float GravityAccel = 0.55f;
        private const float MaxFallSpeed = 24f;

        // Normal homing
        private const float NormalTurnRateDegrees = 14f;
        private const float NormalAcceleration = 0.60f;

        // Aggressive homing (inside 8-tile bubble)
        private const float AggroTurnRateDegrees = 60f;   // almost snap
        private const float AggroAcceleration = 1.50f;
        private const float AggroSpeedCap = 24f;

        // Shared
        private const float MaxHomingSpeed = 19f;
        private const float VerticalBiasStrength = 0.35f;
        private const float FinisherTurnMultiplier = 1.35f;
        private const float FinisherAccelMultiplier = 1.20f;
        private const int Lifetime = 200;
        private const int Pierce = 2;
        private const int TrailDustChance = 5;
        private const int ShatterDust = 8;

        // Targeting
        private const float SearchRadiusPixels = 8f * 16f;   // 8 tiles
        private const bool AlwaysReacquire = true;       // true = dynamic retargeting
        private const bool RequireLineOfSight = false;      // set true if you want tile checks

        // ai0 = original npcIndex (may be -1); ai1 = finisherFlag (1 = last), localAI[0] = elapsed, localAI[1] = lastAcquiredNpc (persist)
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = Pierce;
            Projectile.timeLeft = Lifetime;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            float elapsed = Projectile.localAI[0]++;
            bool isFinisher = Projectile.ai[1] == 1f;

            // Gravity
            Projectile.velocity.Y = Math.Min(Projectile.velocity.Y + GravityAccel, MaxFallSpeed);

            // Acquire target
            int currentStored = (int)Projectile.localAI[1];
            int originalIndex = (int)Projectile.ai[0];
            int targetIndex = AcquireTarget(originalIndex, currentStored);

            Projectile.localAI[1] = targetIndex; // persist chosen target this tick

            NPC target = (targetIndex >= 0) ? Main.npc[targetIndex] : null;

            if (target != null)
            {
                Vector2 toTarget = target.Center - Projectile.Center;

                // Extra vertical bias to encourage diving
                toTarget.Y += Math.Abs(toTarget.Y) * VerticalBiasStrength;

                float distance = toTarget.Length();
                if (distance > 0.0001f)
                {
                    Vector2 desiredDir = toTarget / distance;

                    Vector2 currentDir = Projectile.velocity.LengthSquared() < 0.0001f
                        ? Vector2.UnitY
                        : Projectile.velocity.SafeNormalize(Vector2.UnitY);

                    bool inAggroRange = distance <= SearchRadiusPixels;

                    float turnRateDeg = inAggroRange ? AggroTurnRateDegrees : NormalTurnRateDegrees;
                    float accel = inAggroRange ? AggroAcceleration : NormalAcceleration;
                    float speedCap = inAggroRange ? AggroSpeedCap : MaxHomingSpeed;

                    if (isFinisher)
                    {
                        turnRateDeg *= FinisherTurnMultiplier;
                        accel *= FinisherAccelMultiplier;
                        speedCap += 2f;
                    }

                    float maxTurn = MathHelper.ToRadians(turnRateDeg);
                    float curRot = currentDir.ToRotation();
                    float desRot = desiredDir.ToRotation();
                    float diff = MathHelper.WrapAngle(desRot - curRot);
                    diff = MathHelper.Clamp(diff, -maxTurn, maxTurn);

                    float newRot = curRot + diff;
                    Vector2 newDir = new Vector2((float)Math.Cos(newRot), (float)Math.Sin(newRot));

                    Projectile.velocity += newDir * accel;

                    float speed = Projectile.velocity.Length();
                    if (speed > speedCap)
                        Projectile.velocity *= speedCap / speed;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(TrailDustChance))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.IceTorch,
                    Projectile.velocity.X * 0.18f,
                    Projectile.velocity.Y * 0.22f,
                    150, default,
                    Main.rand.NextFloat(0.85f, 1.25f));
                Main.dust[d].noGravity = true;
            }
        }

        private int AcquireTarget(int originalIndex, int lastIndex)
        {
            // Use last index if still valid & we are not forced to reacquire
            if (!AlwaysReacquire && IsValidTarget(lastIndex))
                return lastIndex;

            int best = -1;
            float bestDistSq = SearchRadiusPixels * SearchRadiusPixels;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!IsValidTarget(i)) continue;

                float distSq = Vector2.DistanceSquared(npc.Center, Projectile.Center);
                if (distSq <= bestDistSq)
                {
                    if (RequireLineOfSight && !Collision.CanHit(Projectile.Center, 1, 1, npc.Center, 1, 1))
                        continue;

                    best = i;
                    bestDistSq = distSq;
                }
            }

            // If nothing in range and original is still valid, fall back
            if (best == -1 && IsValidTarget(originalIndex))
                return originalIndex;

            return best;
        }

        private bool IsValidTarget(int index)
        {
            if (index < 0 || index >= Main.npc.Length) return false;
            NPC n = Main.npc[index];
            return n.active && !n.friendly && n.CanBeChasedBy();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Shatter();
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            Shatter();
        }

        private void Shatter()
        {
            for (int i = 0; i < ShatterDust; i++)
            {
                Vector2 v = Main.rand.NextVector2Circular(3.2f, 3.2f);
                int d = Dust.NewDust(Projectile.Center, 0, 0, DustID.SnowBlock, v.X, v.Y, 150, default, 1.05f);
                Main.dust[d].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 300);
        }
    }
}