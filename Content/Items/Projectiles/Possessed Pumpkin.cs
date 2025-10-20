using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class PossessedPumpkin : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.light = 0.8f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
           Projectile.rotation = Projectile.velocity.ToRotation();
            // Similar homing behavior to FearPumpkin
            float maxDetectRadius = 999f;
            float projSpeed = 15f;

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

            //Projectile.rotation += 0.4f;

            // Enhanced flame effects
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.PurpleTorch, 0f, 0f, 100, default, 2f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.BlueTorch, 0f, 0f, 100, default, 2f);   
            }
        }

        public override void Kill(int timeLeft)
        {
            // Larger explosion effect
            for (int i = 0; i < 50; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.PurpleTorch, speed.X * 7, speed.Y * 7);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.BlueTorch, speed.X * 7, speed.Y * 7);
            }

            // Create larger explosion
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero,
                ProjectileID.JackOLantern, Projectile.damage * 3, Projectile.knockBack * 1.5f, Projectile.owner);
        }
    }
}
