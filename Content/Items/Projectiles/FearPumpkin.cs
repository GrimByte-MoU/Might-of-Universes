using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class FearPumpkin : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Homing behavior
            float maxDetectRadius = 999f;
            float projSpeed = 12f;

            NPC closestNPC = null;
            float closestDistance = 999999f;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy())
                {
                    float distance = Vector2.Distance(npc.Center, Projectile.Center);
                    if (distance < closestDistance && distance < maxDetectRadius)
                    {
                        closestNPC = npc;
                        closestDistance = distance;
                    }
                }
            }

            if (closestNPC != null)
            {
                Vector2 direction = closestNPC.Center - Projectile.Center;
                direction.Normalize();
                Projectile.velocity = (Projectile.velocity * 20f + direction * projSpeed) / 21f;
            }

            // Rotation
            //Projectile.rotation += 0.3f;

            // Create flame effects
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Torch, 0f, 0f, 100, default, 1.5f);
            }
        }

        public override void Kill(int timeLeft)
        {
            // Explosion effect
            for (int i = 0; i < 30; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Torch, speed.X * 5, speed.Y * 5);
            }

            // Create explosion
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero,
                ProjectileID.JackOLantern, Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
    }
}
