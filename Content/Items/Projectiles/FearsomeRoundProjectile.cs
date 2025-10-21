using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class FearsomeRoundProjectile : MoUProjectile
    {
        private NPC targetNPC;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.6f;
            Projectile.extraUpdates = 2;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            // Multi-colored dust effect
            if (Main.rand.NextBool())
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.OrangeTorch, 0f, 0f, 100, default, 1.2f);
                
                if (Main.rand.NextBool())
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                        DustID.GreenTorch, 0f, 0f, 50, default, 0.8f);
                }
            }

            // Homing logic
            if (targetNPC == null || !targetNPC.active)
            {
                float maxDistance = 400f;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly)
                    {
                        float distance = Vector2.Distance(Projectile.Center, npc.Center);
                        if (distance < maxDistance)
                        {
                            maxDistance = distance;
                            targetNPC = npc;
                        }
                    }
                }
            }
            else
            {
                Vector2 toTarget = targetNPC.Center - Projectile.Center;
                toTarget.Normalize();
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, toTarget * 18f, 0.08f);
            }
        }
    }
}
