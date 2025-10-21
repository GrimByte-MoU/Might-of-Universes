using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class OrangePrismaticProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 400;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.damage = (int)(Projectile.damage * 0.9f);
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0.1f);
            float targetRotation = Projectile.velocity.ToRotation();
            //Projectile.rotation = targetRotation;
            
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, 0f, 0f, 0, default, 1f);
            
            float maxDetectRadius = 100f;
            float projSpeed = 15f;
            NPC closestNPC = null;
            float closestDistance = float.MaxValue;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly)
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < maxDetectRadius && distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNPC = npc;
                    }
                }
            }

            if (closestNPC != null)
            {
                Vector2 direction = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = (Projectile.velocity * 20f + direction * projSpeed) / 21f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
        }
    }
}
