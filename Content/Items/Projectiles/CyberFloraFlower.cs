using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class CyberFloraFlower : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.arrow = true;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.scale = 0.75f;
        }

        public override void AI()
        {
            Projectile.rotation += 0.3f;

            NPC target = FindClosestNPC(400f);
            if (target != null)
            {
                Vector2 direction = target.Center - Projectile.Center;
                direction.Normalize();
                
                float homingStrength = 0.15f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * Projectile.velocity.Length(), homingStrength);
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.JungleGrass);
                dust.noGravity = true;
                dust.scale = 0.6f;
            }
        }

        private NPC FindClosestNPC(float maxDistance)
        {
            NPC closestNPC = null;
            float closestDistance = maxDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.CanBeChasedBy() && !npc.dontTakeDamage)
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<NaturesToxin>(), 60);
            
            Player player = Main.player[Projectile.owner];
            int healAmount = (int)(damageDone * 0.01f);
            if (healAmount > 0)
            {
                player.Heal(healAmount);
            }
        }
    }
}