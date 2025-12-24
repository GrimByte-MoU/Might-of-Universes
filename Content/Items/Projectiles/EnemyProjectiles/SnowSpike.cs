using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items. Projectiles.EnemyProjectiles
{
    public class SnowSpike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile. width = 14;
            Projectile.height = 14;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile. alpha = 0;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            // Ice trail
            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile. height,
                    DustID.Ice, 0f, 0f, 100, Color. Cyan, 1.0f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.2f;
            }

            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile. height,
                    DustID.IceTorch, 0f, 0f, 150, default, 0.8f);
                Main.dust[dust].noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 0.4f, 0.6f, 0.8f);
        }

        public override void OnHitPlayer(Player target, Player. HurtInfo info)
        {
            int difficulty = Main.masterMode ? 2 : (Main.expertMode ? 1 :  0);
            int[] frostbiteDuration = { 180, 240, 300 };
            int[] frostburnDuration = { 120, 180, 240 };
            
            target.AddBuff(BuffID.Frostburn, frostburnDuration[difficulty]);
            target.AddBuff(BuffID.Frostburn2, frostbiteDuration[difficulty]);

            // Shatter effect
            for (int i = 0; i < 10; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Ice,
                    velocity.X, velocity.Y, 100, Color.White, 1.3f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 230, 255, 220);
        }
    }
}