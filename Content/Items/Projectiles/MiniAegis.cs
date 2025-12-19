using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content. Items.Buffs;
using System;

namespace MightofUniverses. Content.Items.Projectiles
{
    public class MiniAegis : MoUProjectile
    {
        private enum AIState
        {
            Orbiting,
            Bashing,
            Snapping
        }

        private AIState State
        {
            get => (AIState)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }

        private int BashTarget
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        private float orbitAngle = 0f;
        private int bashCooldown = 0;
        private Vector2 bashStartPos;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID. Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 18000;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (! owner.active || owner.dead)
            {
                owner.ClearBuff(ModContent.BuffType<MiniAegisBuff>());
                return;
            }
            if (owner.HasBuff(ModContent.BuffType<MiniAegisBuff>()))
                Projectile.timeLeft = 2;

            int empowerLevel = (int)Projectile.localAI[0];
            Projectile.minionSlots = 1f + empowerLevel;

            if (Projectile. originalDamage > 0)
            {
                Projectile.damage = Projectile.originalDamage;
            }

            if (bashCooldown > 0)
                bashCooldown--;

            AIStateMachine(owner);
            BlockProjectiles();
            UpdateVisuals();
        }

        private void AIStateMachine(Player owner)
        {
            switch (State)
            {
                case AIState.Orbiting:
                    OrbitBehavior(owner);
                    break;

                case AIState.Bashing:
                    BashBehavior(owner);
                    break;

                case AIState. Snapping:
                    SnapBackBehavior(owner);
                    break;
            }
        }

        private void OrbitBehavior(Player owner)
        {
            float orbitDistance = 80f;
            float orbitSpeed = 0.08f;

            orbitAngle += orbitSpeed;

            Vector2 targetPos = owner.Center + new Vector2(
                (float)Math.Cos(orbitAngle) * orbitDistance,
                (float)Math.Sin(orbitAngle) * orbitDistance
            );

            Vector2 direction = targetPos - Projectile.Center;
            float distance = direction.Length();

            if (distance > 10f)
            {
                direction. Normalize();
                Projectile.velocity = direction * Math.Min(distance * 0.2f, 12f);
            }
            else
            {
                Projectile.velocity *= 0.9f;
            }

            Projectile.rotation += 0.2f;

            if (bashCooldown <= 0)
            {
                NPC target = FindNearbyEnemy(300f);
                if (target != null)
                {
                    State = AIState.Bashing;
                    BashTarget = target.whoAmI;
                    bashStartPos = Projectile.Center;
                    bashCooldown = 30;
                    SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, Projectile.Center);
                }
            }
        }

        private void BashBehavior(Player owner)
        {
            if (! Main.npc[BashTarget].active || Main.npc[BashTarget].life <= 0)
            {
                State = AIState.Snapping;
                return;
            }

            NPC target = Main.npc[BashTarget];
            Vector2 direction = target.Center - Projectile.Center;
            float distance = direction.Length();

            if (distance > 20f)
            {
                direction.Normalize();
                Projectile.velocity = direction * 28f;
            }
            else
            {
                DamageTarget(target, 2);
                State = AIState.Snapping;
            }

            Projectile.rotation += 0.4f;

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.JunglePlants, 0f, 0f, 100, default, 1.8f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
        }

        private void SnapBackBehavior(Player owner)
        {
            float orbitDistance = 80f;
            Vector2 targetPos = owner.Center + new Vector2(
                (float)Math.Cos(orbitAngle) * orbitDistance,
                (float)Math.Sin(orbitAngle) * orbitDistance
            );

            Vector2 direction = targetPos - Projectile.Center;
            float distance = direction.Length();

            if (distance > 15f)
            {
                direction.Normalize();
                Projectile.velocity = direction * 35f;
            }
            else
            {
                State = AIState.Orbiting;
                Projectile.velocity *= 0.8f;
            }

            Projectile.rotation += 0.5f;
        }

        private NPC FindNearbyEnemy(float range)
        {
            NPC closest = null;
            float closestDist = range;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc. CanBeChasedBy() && ! npc. dontTakeDamage)
                {
                    float dist = Vector2.Distance(Projectile.Center, npc.Center);
                    if (dist < closestDist)
                    {
                        closest = npc;
                        closestDist = dist;
                    }
                }
            }

            return closest;
        }

        private void DamageTarget(NPC target, int damageMultiplier)
        {
            Vector2 knockDir = (target.Center - Projectile. Center).SafeNormalize(Vector2.Zero);

            target.StrikeNPC(new NPC.HitInfo
            {
                Damage = Projectile.damage * damageMultiplier,
                Knockback = Projectile.knockBack * damageMultiplier,
                HitDirection = knockDir. X > 0 ? 1 :  -1
            });

            target.velocity += knockDir * 15f;

            SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, target.Center);

            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height,
                    DustID.RainbowMk2, knockDir.X * 4f, knockDir.Y * 4f, 100, default, 2f);
                dust.noGravity = true;
            }
        }

        private void BlockProjectiles()
        {
            int pierceBlocked = (int)(Projectile.localAI[1] == 0 ? 1 :  Projectile.localAI[1]);

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile hostileProj = Main.projectile[i];
                
                if (! hostileProj.active || ! hostileProj.hostile || hostileProj.minion)
                    continue;

                float distToProj = Vector2.Distance(Projectile.Center, hostileProj.Center);
                
                if (distToProj < 80f)
                {
                    bool shouldBlock = false;

                    if (hostileProj.penetrate > 0 && hostileProj.penetrate <= pierceBlocked)
                    {
                        hostileProj.Kill();
                        shouldBlock = true;
                    }
                    else if (hostileProj. penetrate > pierceBlocked)
                    {
                        hostileProj.penetrate -= pierceBlocked;
                        if (hostileProj.penetrate <= 0)
                            hostileProj.Kill();
                        shouldBlock = true;
                    }
                    else if (hostileProj.penetrate == -1)
                    {
                        hostileProj.Kill();
                        shouldBlock = true;
                    }

                    if (shouldBlock)
                    {
                        SoundEngine.PlaySound(SoundID.NPCHit4, Projectile.Center);

                        for (int d = 0; d < 12; d++)
                        {
                            Dust dust = Dust.NewDustDirect(hostileProj.position, hostileProj.width, hostileProj.height,
                                DustID.RainbowMk2, 0f, 0f, 100, default, 1.5f);
                            dust.noGravity = true;
                            dust.velocity = (dust.position - Projectile.Center).SafeNormalize(Vector2.Zero) * 3f;
                        }
                    }
                }
            }
        }

        private void UpdateVisuals()
        {
            Color lightColor = new Color(1f, 0.7f, 0.4f);
            Lighting.AddLight(Projectile. Center, lightColor. ToVector3() * 1.2f);

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.JunglePlants, 0f, 0f, 100, default, 1.3f);
                dust.noGravity = true;
                dust.velocity *= 0.2f;
            }

            if (State == AIState.Bashing && Main.rand.NextBool())
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.RainbowMk2, 0f, 0f, 100, new Color(1f, 0.8f, 0.3f), 1.8f);
                dust.noGravity = true;
            }
        }
    }
}