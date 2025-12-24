using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft. Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses. Content.Items.Projectiles
{
    public class FearsomeRoundProjectile : MoUProjectile
    {
        private NPC targetNPC;
        private int homingDelay = 0;

        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass. Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.6f;
            Projectile.extraUpdates = 2;
            // REMOVED AIType - this was overriding your custom AI! 
        }

        public override void AI()
        {
            // Dust trail
            if (Main.rand.NextBool())
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile. height, 
                    DustID.OrangeTorch, 0f, 0f, 100, default, 1.2f);
                
                if (Main.rand. NextBool())
                {
                    Dust. NewDust(Projectile. position, Projectile.width, Projectile.height, 
                        DustID.GreenTorch, 0f, 0f, 50, default, 0.8f);
                }
            }

            // Rotation follows velocity
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // Short delay before homing starts (5 frames = 0.08 seconds)
            homingDelay++;
            if (homingDelay < 5) return;

            // Find or validate target
            if (targetNPC == null || !targetNPC. active || targetNPC.life <= 0)
            {
                targetNPC = FindTarget();
            }

            // Home in on target
            if (targetNPC != null)
            {
                HomeTowardsTarget();
            }
        }

        private NPC FindTarget()
        {
            float maxDetectionRange = 500f; // Larger than Chlorophyte (400f)
            float closestDistance = maxDetectionRange;
            NPC closestNPC = null;

            Vector2 currentDirection = Projectile.velocity.SafeNormalize(Vector2.UnitX);

            foreach (NPC npc in Main.npc)
            {
                if (! npc.active || npc. friendly || ! npc.CanBeChasedBy() || npc. life <= 0)
                    continue;

                Vector2 toNPC = npc.Center - Projectile.Center;
                float distance = toNPC.Length();

                if (distance > maxDetectionRange)
                    continue;

                // Check if target is in front (more lenient than before)
                Vector2 directionToNPC = toNPC.SafeNormalize(Vector2.UnitX);
                float angle = Vector2.Dot(currentDirection, directionToNPC);

                // 0.3 = ~72° cone (more lenient than Chlorophyte's ~60°)
                if (angle > 0.3f && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestNPC = npc;
                }
            }

            return closestNPC;
        }

        private void HomeTowardsTarget()
        {
            Vector2 toTarget = targetNPC.Center - Projectile.Center;
            float distance = toTarget.Length();

            // Lose target if it goes too far or behind
            if (distance > 600f)
            {
                targetNPC = null;
                return;
            }

            Vector2 currentDirection = Projectile.velocity. SafeNormalize(Vector2.UnitX);
            Vector2 desiredDirection = toTarget.SafeNormalize(Vector2.UnitX);

            // Lose target if it's too far behind (45° = 0.7)
            float angle = Vector2.Dot(currentDirection, desiredDirection);
            if (angle < 0.2f) // More aggressive - keeps tracking longer
            {
                targetNPC = null;
                return;
            }

            // Homing strength
            float speed = Projectile.velocity.Length();
            float maxSpeed = 20f; // Faster than Chlorophyte (16f)
            float homingStrength = 0.15f; // More aggressive than Chlorophyte (0.08f)

            // Accelerate towards target
            Vector2 desiredVelocity = desiredDirection * maxSpeed;
            Projectile.velocity = Vector2.Lerp(Projectile. velocity, desiredVelocity, homingStrength);

            // Ensure minimum speed
            if (Projectile.velocity.Length() < 14f)
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX) * 14f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent. BuffType<Terrified>(), 180);

            // Impact dust
            for (int i = 0; i < 10; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.OrangeTorch,
                    velocity.X, velocity.Y, 100, default, 1.5f);
                dust.noGravity = true;
            }
        }
    }
}