using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common.Players; // For Overclock suppression

namespace MightofUniverses.Content.Items.Projectiles
{
    public class HellsEyeMinion : MoUProjectile
    {
        private const float PlayerDetectRangePx = 50f * 16f;
        private const float OrbitRadius = 96f;
        private const float IdleRadius = 64f;
        private const int ShootCooldownTicks = 24; // 1.5 shots/sec per eye
        private const float LaserSpeed = 18f;
        private const float LaserDamageMult = 0.65f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true; // right-click target support
        }

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.scale = 1.5f;

            Projectile.timeLeft = 18000;
            Projectile.minionSlots = 0.25f;
            Projectile.usesLocalNPCImmunity = false;
            Projectile.damage = 0;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // Keep alive while buff is active
            if (player.HasBuff(ModContent.BuffType<HellsEyeBuff>()))
                Projectile.timeLeft = 2;

            // Acquire target: prefer player's marked target
            NPC target = GetTarget(player);

            // Find index among owner's eyes for spacing
            int total = player.ownedProjectileCounts[Type];
            int index = GetOwnIndex();

            // Movement
            Vector2 desiredPos;
            float inertia;
            float speed;

            if (target != null && Vector2.DistanceSquared(player.Center, target.Center) <= PlayerDetectRangePx * PlayerDetectRangePx)
            {
                // Orbit the enemy
                float angle = (float)(Main.GameUpdateCount * 0.05f) + (index * MathHelper.TwoPi / Math.Max(1, total));
                desiredPos = target.Center + OrbitRadius * angle.ToRotationVector2();
                speed = 18f;
                inertia = 20f;

                // Face the target
                Vector2 toTarget = target.Center - Projectile.Center;
                if (toTarget.LengthSquared() > 0.001f)
                {
                    Projectile.rotation = toTarget.ToRotation();
                    Projectile.spriteDirection = Projectile.rotation > MathHelper.PiOver2 || Projectile.rotation < -MathHelper.PiOver2 ? -1 : 1;
                }

                // Shooting
                DoShooting(player, target);
            }
            else
            {
                // Idle swarm near player in a loose ring with a little wobble
                float wobble = (float)Math.Sin((Main.GameUpdateCount + index * 13) * 0.08f) * 14f;
                float angle = (float)(Main.GameUpdateCount * 0.03f) + (index * MathHelper.TwoPi / Math.Max(1, total));
                desiredPos = player.Center + (IdleRadius + wobble) * angle.ToRotationVector2();
                speed = 14f;
                inertia = 15f;
                // Face outward slightly
                Vector2 outward = desiredPos - Projectile.Center;
                if (outward.LengthSquared() > 0.001f)
                {
                    Projectile.rotation = outward.ToRotation();
                    Projectile.spriteDirection = Projectile.rotation > MathHelper.PiOver2 || Projectile.rotation < -MathHelper.PiOver2 ? -1 : 1;
                }
                if (Projectile.localAI[0] > 0) Projectile.localAI[0]--;
            }

            // Smooth move toward desiredPos
            Vector2 toDesired = desiredPos - Projectile.Center;
            if (toDesired.Length() > 200f) // snap faster if far
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, toDesired.SafeNormalize(Vector2.Zero) * speed, 0.25f);
            }
            else
            {
                Vector2 wantVel = toDesired.SafeNormalize(Vector2.Zero) * speed;
                Projectile.velocity = (Projectile.velocity * (inertia - 1f) + wantVel) / inertia;
            }

            // Small light/visual
            Lighting.AddLight(Projectile.Center, 0.25f, 0.05f, 0.05f);
        }

        private NPC GetTarget(Player player)
        {
            // Right-clicked target
            if (player.MinionAttackTargetNPC >= 0)
            {
                NPC forced = Main.npc[player.MinionAttackTargetNPC];
                if (forced.CanBeChasedBy(this))
                    return forced;
            }

            // Closest enemy to player within detect range
            NPC chosen = null;
            float best = PlayerDetectRangePx * PlayerDetectRangePx;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.CanBeChasedBy(this)) continue;
                float d2 = Vector2.DistanceSquared(player.Center, npc.Center);
                if (d2 < best)
                {
                    best = d2;
                    chosen = npc;
                }
            }
            return chosen;
        }

        private int GetOwnIndex()
        {
            int idx = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (other.active && other.owner == Projectile.owner && other.type == Projectile.type)
                {
                    if (other.whoAmI == Projectile.whoAmI)
                        return idx;
                    idx++;
                }
            }
            return 0;
        }

        private void DoShooting(Player player, NPC target)
        {
            var mou = player.GetModPlayer<MOUPlayer>();
            if (mou != null && mou.MinionAttacksSuppressed)
                return;

            if (Projectile.localAI[0] > 0)
            {
                Projectile.localAI[0]--;
                return;
            }

            // Fire if in reasonable range and line-of-sight is clear enough
            Vector2 toTarget = target.Center - Projectile.Center;
            float dist = toTarget.Length();
            if (dist <= 700f)
            {
                toTarget.Normalize();
                Vector2 vel = toTarget * LaserSpeed;

                int dmg = Math.Max(1, (int)Math.Round((Projectile.originalDamage > 0 ? Projectile.originalDamage : 20) * LaserDamageMult));
                int p = Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    vel,
                    ModContent.ProjectileType<HellsEyeLaser>(),
                    dmg,
                    0f,
                    Projectile.owner
                );
                if (p >= 0 && p < Main.maxProjectiles)
                {
                    Main.projectile[p].originalDamage = dmg;
                }

                SoundEngine.PlaySound(SoundID.Item12 with { Volume = 0.7f, PitchVariance = 0.2f }, Projectile.Center);
                Projectile.localAI[0] = ShootCooldownTicks;
            }
        }
    }
}