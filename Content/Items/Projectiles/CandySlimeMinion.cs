using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CandySlimeMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 0.66f; // 3 slimes = 2 slots
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.HasBuff(ModContent.BuffType<CandySlimeBuff>()))
            {
                Projectile.Kill();
                return;
            }

            // Teleport to player if too far away
            if (Vector2.Distance(player.Center, Projectile.Center) > 800f)
            {
                Projectile.Center = player.Center;
                Projectile.velocity *= 0.1f;
            }

            Lighting.AddLight(Projectile.Center, Color.Pink.ToVector3() * 0.5f);

            // Basic minion AI
            Vector2 targetCenter = Projectile.Center;
            bool foundTarget = false;
            float distanceFromTarget = 700f;
            
            // Look for enemies
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy())
                {
                    float between = Vector2.Distance(npc.Center, Projectile.Center);
                    bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                    bool inRange = between < distanceFromTarget;
                    
                    if ((closest && inRange) || !foundTarget)
                    {
                        distanceFromTarget = between;
                        targetCenter = npc.Center;
                        foundTarget = true;
                    }
                }
            }

            // Movement behavior
            if (foundTarget)
            {
                // Chase target
                Vector2 direction = targetCenter - Projectile.Center;
                direction.Normalize();
                Projectile.velocity = (Projectile.velocity * 20f + direction * 8f) / 21f;
            }
            else
            {
                // Follow player
                Vector2 idlePosition = player.Center;
                idlePosition.Y -= 48f;
                
                float distance = Vector2.Distance(idlePosition, Projectile.Center);
                if (distance > 32f)
                {
                    Vector2 direction = idlePosition - Projectile.Center;
                    direction.Normalize();
                    Projectile.velocity = (Projectile.velocity * 20f + direction * 4f) / 21f;
                }
                else
                {
                    Projectile.velocity *= 0.96f;
                }
            }

            //Projectile.rotation += 0.05f * Projectile.direction;
        }
    }
}
