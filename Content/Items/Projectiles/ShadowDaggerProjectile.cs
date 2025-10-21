using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ShadowDaggerProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            // Clone Shadowflame Knife projectile
            Projectile.CloneDefaults(ProjectileID.ShadowFlameKnife);
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            AIType = ProjectileID.ShadowFlameKnife; // Use same AI
        }

        private int bounceCount = 0;
        private const int MaxBounces = 4;

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += 25;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Inflict Shadowflame for 5-10 seconds
            int debuffDuration = Main.rand.Next(300, 601);
            target.AddBuff(BuffID.ShadowFlame, debuffDuration);

            // Ricochet to another enemy if bounces remain
            if (bounceCount < MaxBounces)
            {
                bounceCount++;
                NPC nextTarget = FindNearestEnemy(target);
                
                if (nextTarget != null)
                {
                    // Home towards next target
                    Vector2 direction = nextTarget.Center - Projectile.Center;
                    direction.Normalize();
                    Projectile.velocity = direction * Projectile.velocity.Length();
                    
                    // Visual effect
                    for (int i = 0; i < 8; i++)
                    {
                        Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, 0f, 0f, 100, default, 1.5f);
                        dust.noGravity = true;
                        dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                    }
                }
            }
        }

        private NPC FindNearestEnemy(NPC excludeNPC)
        {
            NPC closestNPC = null;
            float closestDistance = 500f;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage && npc.CanBeChasedBy() && npc.whoAmI != excludeNPC.whoAmI)
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNPC = npc;
                    }
                }
            }

            return closestNPC;
        }
    }
}