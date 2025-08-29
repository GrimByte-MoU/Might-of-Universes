using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class PrismaticGrenade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 19;
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
                DustID.RainbowTorch, 0f, 0f, 50, default, 0.8f);
            
            float rainbowValue = (float)Main.time * 0.1f;
            float r = Main.DiscoR / 255f;
            float g = Main.DiscoG / 255f;
            float b = Main.DiscoB / 255f;
            Lighting.AddLight(Projectile.Center, r * 0.8f, g * 0.8f, b * 0.8f);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 50; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                    DustID.RainbowTorch, speed.X * 5f, speed.Y * 5f, 20, default, 1.2f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<PrismaticRend>(), 120);
            target.AddBuff(ModContent.BuffType<GoblinsCurse>(), 180);
        }
    }
}