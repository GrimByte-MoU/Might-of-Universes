using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Common.Players
{
    public class SkeletalCoveringPlayer : ModPlayer
    {
        public bool hasSkeletalCovering;
        private int attackTimer;
        private int rapidFireTime;

        public override void ResetEffects()
        {
            hasSkeletalCovering = false;
        }

        public override void UpdateDead()
        {
            hasSkeletalCovering = false;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (hasSkeletalCovering)
            {
                rapidFireTime = 30;
            }
        }

        public override void PostUpdate()
        {
            if (!hasSkeletalCovering)
                return;

            attackTimer++;

            if (rapidFireTime > 0)
            {
                rapidFireTime--;

                if (attackTimer >= 6)
                {
                    attackTimer = 0;
                    FireBone(Player, true); // Use max damage burst
                }
                return;
            }

            // Find the closest target
            NPC target = null;
            float minDistance = float.MaxValue;

            foreach (var npc in Main.npc)
            {
                if (npc.CanBeChasedBy(Player))
                {
                    float distance = Vector2.Distance(npc.Center, Player.Center);
                    if (distance < minDistance)
                    {
                        target = npc;
                        minDistance = distance;
                    }
                }
            }

            if (target == null)
                return;

            int interval = minDistance switch
            {
                <= 160f => 15,  // <10 tiles
                <= 320f => 20,  // 11–20 tiles
                <= 480f => 30,  // 21–30 tiles
                _ => 40         // >30 tiles
            };

            if (attackTimer >= interval)
            {
                attackTimer = 0;
                FireBone(Player, false);
            }
        }

        private void FireBone(Player player, bool rapidFire)
        {
            Vector2 position = player.Center;
            Vector2 target = Vector2.Zero;
            float closestDistance = 480f;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(player))
                {
                    float dist = Vector2.Distance(npc.Center, position);
                    if (dist < closestDistance)
                    {
                        closestDistance = dist;
                        target = npc.Center;
                    }
                }
            }

            if (closestDistance == 480f)
                return;

            Vector2 direction = (target - position).SafeNormalize(Vector2.UnitX) * 8f;

            // Choose projectile type and damage
            int projType = ModContent.ProjectileType<CrossboneProjectile>();
            int damage = 15;

            if (Main.rand.NextBool(2))
            {
                projType = ModContent.ProjectileType<HomingSkullProjectile>();
                damage = 25;
            }
            if (Main.rand.NextBool(2))
            {
                projType = ModContent.ProjectileType<BlueSkullProjectile>();
                damage = 35;
            }

            // Boost damage slightly during rapid fire burst
            if (rapidFire)
            {
                damage += 10;
            }

            float knockback = 1f;

            Projectile.NewProjectile(
                player.GetSource_FromThis(),
                position,
                direction,
                projType,
                damage,
                knockback,
                player.whoAmI
            );
        }
    }
}

