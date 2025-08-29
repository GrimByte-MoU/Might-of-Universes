using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class BlueSkullProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.DamageType = ModContent.GetInstance<PacifistDamageClass>();
            Projectile.damage = 35;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            float maxDetectRadius = 500f;
            float homingSpeed = 6f;

            NPC target = FindClosestEnemy(maxDetectRadius);
            if (target != null)
            {
                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * homingSpeed, 0.08f);
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        private NPC FindClosestEnemy(float maxDetectDistance)
        {
            NPC closest = null;
            float minDist = maxDetectDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(Projectile))
                {
                    float dist = Vector2.Distance(Projectile.Center, npc.Center);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closest = npc;
                    }
                }
            }

            return closest;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Vector2 velocity = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 6f) * 5f;
                Projectile.NewProjectile(
                    Projectile.GetSource_Death(),
                    Projectile.Center,
                    velocity,
                    ModContent.ProjectileType<BlueFireProjectile>(),
                    10,
                    0f,
                    Projectile.owner
                );
            }
        }
    }
}

