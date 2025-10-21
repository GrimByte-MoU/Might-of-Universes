using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ToxicGrenadeProjectile : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 9;
            Projectile.light = 0.8f;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < 120)
            {
                Projectile.velocity.Y += 0.2f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                DustID.CursedTorch, 0f, 0f, 100, default, 1.2f);
            
            Lighting.AddLight(Projectile.Center, 0f, 0.8f, 0f);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 50; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.CursedTorch, speed.X * 5f, speed.Y * 5f, 100, default, 2f);
            }
        }
            
public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                target.AddBuff(ModContent.BuffType<GoblinsCurse>(), 120);
                target.AddBuff(BuffID.Venom, 180);
                
            
        }
    }
    }
    

