using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class GildedSlitterProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 113;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 9;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            
            // Create spirit trail effect
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                DustID.SpectreStaff, 0f, 0f, 100, Color.Lime, 1.2f);
            dust.noGravity = true;
            dust.velocity *= 0.3f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Stick in the enemy
            if (Projectile.penetrate > 0)
            {
                Projectile.ai[0] = 1f;
                Projectile.ai[1] = target.whoAmI;
                Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
                Projectile.netUpdate = true;
                
                // Create impact effect
                for (int i = 0; i < 15; i++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                        DustID.SpectreStaff, hit.HitDirection * 2.5f, -2.5f, 0, Color.Gold, 1.2f);
                }
            }
        }
    }
}
