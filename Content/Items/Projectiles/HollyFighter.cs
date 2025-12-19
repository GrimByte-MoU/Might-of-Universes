using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using MightofUniverses. Content.Items.Buffs;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class HollyFighter : MoUProjectile
    {
        private const float SHOOT_RATE = 15f;
        private float attackTimer = 0f;

        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets. CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile. minionSlots = 1f;
            Projectile. penetrate = -1;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile. owner];

            if (!CheckActive(owner))
                return;

            GeneralBehavior(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
            SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
            Movement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
            Attack(foundTarget, distanceFromTarget, targetCenter);

            HandleVisuals();
        }

        private void HandleVisuals()
        {
            float velocityThreshold = 2f;
            
            if (Projectile.velocity.X > velocityThreshold)
                Projectile.spriteDirection = 1;
            else if (Projectile.velocity.X < -velocityThreshold)
                Projectile.spriteDirection = -1;

            Projectile.rotation = 0f;

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.BlueTorch, 0f, 0f, 100, default, 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.2f;
                dust.scale = Main.rand.NextFloat(0.8f, 1.3f);
            }

            Lighting.AddLight(Projectile.Center, 0.5f, 0.7f, 1f);
        }

        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<HollyFighterBuff>());
                return false;
            }

            if (owner. HasBuff(ModContent.BuffType<HollyFighterBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }

        private void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition)
        {
            Vector2 idlePosition = owner. Center;
            idlePosition. Y -= 96f;

            vectorToIdlePosition = idlePosition - Projectile.Center;
            distanceToIdlePosition = vectorToIdlePosition. Length();

            if (Main. myPlayer == owner.whoAmI && distanceToIdlePosition > 2000f)
            {
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile. netUpdate = true;
            }
        }

        private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
        {
            distanceFromTarget = 700f;
            targetCenter = Projectile.position;
            foundTarget = false;

            if (owner. HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);
                if (between < 2000f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }

            if (!foundTarget)
            {
                foreach (var npc in Main.ActiveNPCs)
                {
                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc. position, npc.width, npc.height);

                        if (((closest && inRange) || !foundTarget) && lineOfSight)
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }
        }

        private void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {
            float speed = 16f;
            float inertia = 20f;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (other. active && other.type == Projectile.type && other.owner == Projectile.owner && other.whoAmI != Projectile.whoAmI)
                {
                    Vector2 distanceToOther = Projectile.Center - other.Center;
                    float distance = distanceToOther.Length();
                    if (distance < 60f && distance > 0f)
                    {
                        distanceToOther.Normalize();
                        Projectile.velocity += distanceToOther * 0.05f;
                    }
                }
            }

            if (Projectile.velocity.Length() > speed * 1.3f)
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * speed * 1.3f;
            }

            if (foundTarget)
            {
                Vector2 targetPosition = targetCenter;
                
                float strafingOffset = (float)Math.Sin(Main.GameUpdateCount * 0.03f + Projectile.whoAmI * 0.8f) * 120f;
                targetPosition.X += strafingOffset;
                targetPosition.Y -= 100f + (Projectile.whoAmI % 3) * 40f;
                
                Vector2 direction = targetPosition - Projectile.Center;
                float distanceToTarget = direction.Length();

                if (distanceToTarget > 30f)
                {
                    direction.Normalize();
                    direction *= speed;
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
                }
                else
                {
                    Projectile.velocity *= 0.96f;
                }
            }
            else
            {
                Vector2 idleOffset = new Vector2(
                    (float)Math.Cos(Main.GameUpdateCount * 0.02f + Projectile.whoAmI * 0.5f) * 80f,
                    (float)Math.Sin(Main.GameUpdateCount * 0.015f + Projectile.whoAmI * 0.7f) * 40f
                );
                Vector2 targetIdlePos = vectorToIdlePosition + idleOffset;

                if (targetIdlePos.Length() > 20f)
                {
                    targetIdlePos.Normalize();
                    targetIdlePos *= speed * 0.8f;
                    Projectile.velocity = (Projectile. velocity * (inertia - 1) + targetIdlePos) / inertia;
                }
                else
                {
                    Projectile.velocity *= 0.96f;
                }
            }
        }

        private void Attack(bool foundTarget, float distanceFromTarget, Vector2 targetCenter)
        {
            attackTimer++;
            if (foundTarget && attackTimer >= SHOOT_RATE)
            {
                attackTimer = 0f;
                Vector2 direction = (targetCenter - Projectile.Center).SafeNormalize(Vector2.Zero);
                
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    direction * 20f,
                    ModContent.ProjectileType<MistletoeThorn>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );

                SoundEngine.PlaySound(SoundID.Item17, Projectile.Center);
            }
        }
    }
}