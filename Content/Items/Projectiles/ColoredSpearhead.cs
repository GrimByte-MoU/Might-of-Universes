using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ColoredSpearhead : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Home in on enemies
            NPC target = null;
            float distance = 1000f;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].CanBeChasedBy(this))
                {
                    float targetDistance = Vector2.Distance(Projectile.position, Main.npc[i].position);
                    if (targetDistance < distance)
                    {
                        target = Main.npc[i];
                        distance = targetDistance;
                    }
                }
            }

            if (target != null)
            {
                Vector2 direction = target.position - Projectile.position;
                direction.Normalize();
                Projectile.velocity = direction * 15f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
        }
    }
}