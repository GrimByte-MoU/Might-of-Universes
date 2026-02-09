using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class TitanoboaMini : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.TinyEater);
            AIType = ProjectileID.TinyEater;
            Projectile.penetrate = 8;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        private int bounceCount = 0;
        private const int MaxBounces = 7;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Tarred>(), 180);
            if (bounceCount < MaxBounces)
            {
                bounceCount++;
                NPC nextTarget = FindNearestEnemy(target);
                
                if (nextTarget != null)
                {
                    Vector2 direction = nextTarget.Center - Projectile.Center;
                    direction.Normalize();
                    Projectile.velocity = direction * 12f;
                }
            }
        }

        private NPC FindNearestEnemy(NPC excludeNPC)
        {
            NPC closestNPC = null;
            float closestDistance = 400f;

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