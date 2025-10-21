using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ClockworkAirship : MoUProjectile
    {
        private int attackCooldown = 0;
        private int bombCooldown = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SafeSetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 3f;
            Projectile.penetrate = -1;
            Projectile.scale = 3f;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (!CheckActive(owner)) return;

            GeneralBehavior(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
            SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
            Movement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
            Attack(foundTarget, targetCenter);
            Visuals();
        }

        private void Attack(bool foundTarget, Vector2 targetCenter)
        {
            if (foundTarget)
            {
                if (attackCooldown <= 0)
                {
                    Vector2 fireboltPosition = Projectile.Center + new Vector2(0, -20f); // Just above center
                    Vector2 direction = (targetCenter - fireboltPosition).SafeNormalize(Vector2.Zero) * 12f;

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        fireboltPosition,
                        direction,
                        ModContent.ProjectileType<ClockworkFirebolt>(),
                        Projectile.damage,
                        Projectile.knockBack,
                        Projectile.owner
                    );

                    attackCooldown = 5;
                }

                if (bombCooldown <= 0)
                {
                    Vector2 bombPosition = Projectile.Center + new Vector2(0, 10f); // Slightly below center
                    Vector2 direction = (targetCenter - bombPosition).SafeNormalize(Vector2.Zero) * 12f;

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        bombPosition,
                        direction,
                        ModContent.ProjectileType<ClockworkBomb>(),
                        Projectile.damage * 5,
                        Projectile.knockBack * 5,
                        Projectile.owner
                    );

                    bombCooldown = 120;
                }
            }

            attackCooldown--;
            bombCooldown--;
        }

        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<ClockworkAirshipBuff>());
                return false;
            }

            if (owner.HasBuff(ModContent.BuffType<ClockworkAirshipBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }

        private void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition)
        {
            Vector2 idlePosition = owner.Center;
            idlePosition.Y -= 180f;
            idlePosition.X += 40f * -owner.direction;

            vectorToIdlePosition = idlePosition - Projectile.Center;
            distanceToIdlePosition = vectorToIdlePosition.Length();

            if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2000f)
            {
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }
        }

        private void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {
            float speed = 12f;
            float inertia = 8f;

            if (foundTarget)
            {
                if (distanceFromTarget > 300f)
                {
                    Vector2 direction = targetCenter - Projectile.Center;
                    direction.Normalize();
                    direction *= speed;
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
                }
            }
            else
            {
                if (distanceToIdlePosition > 20f)
                {
                    vectorToIdlePosition.Normalize();
                    vectorToIdlePosition *= speed;
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                }
            }
        }

        private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
        {
            distanceFromTarget = 700f;
            targetCenter = Projectile.position;
            foundTarget = false;

            if (owner.HasMinionAttackTargetNPC)
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
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);

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

        private void Visuals()
        {
            if (Projectile.velocity.X != 0)
            {
                Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            }

            Projectile.rotation = 0f;
            Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3() * 0.78f);
        }
    }
}

