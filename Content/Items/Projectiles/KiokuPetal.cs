using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class KiokuPetal : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 90;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            NPC target = null;
            float distanceMax = 400f;
            Lighting.AddLight(Projectile.Center, 1.0f, 0.6f, 0.8f);


            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.CanBeChasedBy(this))
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < distanceMax)
                    {
                        distanceMax = distance;
                        target = npc;
                    }
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (target != null)
            {
                Vector2 move = target.Center - Projectile.Center;
                move.Normalize();
                Projectile.velocity = (Projectile.velocity * 20f + move * 6f) / 21f;
            }
        }
        
    }
}
