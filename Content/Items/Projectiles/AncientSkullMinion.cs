using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class AncientSkullMinion : ModProjectile
    {
        // Behavior constants
        private const float AcquireRangePx = 960f;
        private const float IdleOffsetY = -60f;
        private const float AboveTargetOffsetY = -120f;
        private const float MoveLerp = 0.12f;
        private const int Stage1End = 180;
        private const int Stage2End = 360;
        private const int Stage3End = 960;

        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 18000;

            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.netImportant = true;
        }

        public override bool MinionContactDamage() => false;

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead)
            {
                owner.ClearBuff(ModContent.BuffType<AncientSkullMinionBuff>());
                return;
            }
            if (owner.HasBuff(ModContent.BuffType<AncientSkullMinionBuff>()))
                Projectile.timeLeft = 2;

            // Acquire or maintain target
            int currentTarget = (int)Projectile.ai[0] - 1;
            NPC target = (currentTarget >= 0 && currentTarget < Main.maxNPCs) ? Main.npc[currentTarget] : null;
            if (target == null || !target.active || target.friendly || target.dontTakeDamage || Vector2.DistanceSquared(owner.Center, target.Center) > AcquireRangePx * AcquireRangePx)
            {
                // find nearest
                float bestDist = AcquireRangePx;
                int bestIdx = -1;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC n = Main.npc[i];
                    if (!n.active || n.friendly || n.dontTakeDamage || n.life <= 0)
                        continue;
                    float d = Vector2.Distance(owner.Center, n.Center);
                    if (d <= bestDist)
                    {
                        bestDist = d;
                        bestIdx = i;
                    }
                }
                if (bestIdx != -1)
                {
                    Projectile.ai[0] = bestIdx + 1;
                    Projectile.localAI[0] = 0f; // reset lock timer
                    Projectile.netUpdate = true;
                    target = Main.npc[bestIdx];
                }
                else
                {
                    Projectile.ai[0] = 0f;
                    target = null;
                }
            }
            else
            {
                // If there is a closer enemy within range, switch
                float currentDist = Vector2.Distance(owner.Center, target.Center);
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC n = Main.npc[i];
                    if (!n.active || n.friendly || n.dontTakeDamage || n.life <= 0 || n.whoAmI == target.whoAmI)
                        continue;
                    float d = Vector2.Distance(owner.Center, n.Center);
                    if (d < currentDist && d <= AcquireRangePx)
                    {
                        Projectile.ai[0] = n.whoAmI + 1;
                        Projectile.localAI[0] = 0f; // reset lock timer
                        Projectile.netUpdate = true;
                        target = n;
                        break;
                    }
                }
            }

            // Determine formation index so multiple minions spread horizontally
            (int index, int count) = GetFormationIndex(owner);
            float spread = 60f; // horizontal spacing between minions
            float sideOffset = (index - (count - 1) * 0.5f) * spread;

            // Subtle bob so it isn’t rigid
            float bob = (float)Math.Sin((Main.GameUpdateCount * 0.12f) + Projectile.whoAmI * 0.7f) * 8f;

            Vector2 desiredPos;
            if (target == null)
            {
                // Idle near player, offset to side for formations
                desiredPos = owner.Center + new Vector2(sideOffset, IdleOffsetY + bob);
                Projectile.rotation = 0f;
                Projectile.localAI[1] = 0f; // beam accumulator
                Projectile.localAI[0] = 0f; // lock timer
            }
            else
            {
                // Hold above and to the side of the target
                desiredPos = target.Center + new Vector2(sideOffset, AboveTargetOffsetY + bob);
                Vector2 toTarget = target.Center - Projectile.Center;
                Projectile.rotation = toTarget.ToRotation();

                // Increase lock time
                Projectile.localAI[0] += 1f;

                // Beam damage ticking
                DoBeamDamage(owner, target);
                // Dust visuals along the beam line
                BeamVisuals(target);
            }

            // Movement
            Vector2 to = desiredPos - Projectile.Center;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, to, MoveLerp);
        }

        private (int index, int count) GetFormationIndex(Player owner)
        {
            int count = 0;
            int index = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (!p.active || p.owner != owner.whoAmI || p.type != ModContent.ProjectileType<AncientSkullMinion>())
                    continue;
                if (i == (int)p.whoAmI)
                {
                    // not how we compare; compute order by whoAmI ascending
                }
            }

            // Order by whoAmI to produce a consistent index
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (!p.active || p.owner != owner.whoAmI || p.type != ModContent.ProjectileType<AncientSkullMinion>())
                    continue;

                if (i < Projectile.whoAmI)
                    index++;
                count++;
            }
            if (count == 0) count = 1; // avoid divide by zero
            return (index, count);
        }

        private void DoBeamDamage(Player owner, NPC target)
        {
            // Determine stage based on lock time
            int lockTicks = (int)Projectile.localAI[0];
            float mult;
            float hitsPerSecond;

            if (lockTicks < Stage1End)
            {
                mult = 0.50f;
                hitsPerSecond = 3f;
            }
            else if (lockTicks < Stage2End)
            {
                mult = 0.75f;
                hitsPerSecond = 5f;
            }
            else if (lockTicks < Stage3End)
            {
                mult = 1.00f;
                hitsPerSecond = 8f;
            }
            else
            {
                mult = 1.25f;
                hitsPerSecond = 10f;
            }

            float interval = 60f / hitsPerSecond;
            Projectile.localAI[1] += 1f;

            while (Projectile.localAI[1] >= interval)
            {
                Projectile.localAI[1] -= interval;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    return;

                // Compute summon damage with owner's modifiers
                var mod = owner.GetTotalDamage(DamageClass.Summon);
                int dmg = (int)mod.ApplyTo(Projectile.originalDamage * mult);

                int dir = (target.Center.X > Projectile.Center.X) ? 1 : -1;

                var info = new NPC.HitInfo
                {
                    Damage = dmg,
                    Knockback = 0f,
                    HitDirection = dir,
                    DamageType = DamageClass.Summon
                };
                try
                {
                    target.StrikeNPC(info);
                }
                catch
                {
                    // NPC may have died between scheduling and tick
                }
            }
        }

        private void BeamVisuals(NPC target)
        {
            // Draw a line of dark smoke-like dust between skull and target
            Vector2 start = Projectile.Center;
            Vector2 end = target.Center;
            Vector2 delta = end - start;
            float len = delta.Length();
            if (len <= 2f) return;

            Vector2 step = delta / 12f; // number of samples
            Vector2 p = start;
            byte alphaBase = 140;

            int lockTicks = (int)Projectile.localAI[0];
            if (lockTicks > Stage3End) alphaBase = 240;
            else if (lockTicks > Stage2End) alphaBase = 210;
            else if (lockTicks > Stage1End) alphaBase = 180;

            for (int i = 0; i < 12; i++)
            {
                int d = Dust.NewDust(p - new Vector2(2), 4, 4, DustID.Smoke, 0f, 0f, alphaBase, new Color(60, 60, 60), 1.1f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.1f;
                p += step;
            }
        }

        public override bool? CanCutTiles() => false;

        public override bool? CanDamage() => false;

        public override void OnKill(int timeLeft)
        {
            // light poof
            for (int i = 0; i < 12; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, Scale: Main.rand.NextFloat(1f, 1.4f));
                Main.dust[d].noGravity = true;
            }
        }
    }
}