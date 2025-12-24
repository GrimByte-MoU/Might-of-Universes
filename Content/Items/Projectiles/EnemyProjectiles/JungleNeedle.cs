using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles.EnemyProjectiles
{
    public class JungleNeedle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            // Point in direction of travel
            Projectile.rotation = Projectile. velocity.ToRotation();

            // Poison drip trail
            if (Main.rand. NextBool(3))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.JungleSpore, 0f, 0f, 100, Color.LimeGreen, 0.9f);
                Main. dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.2f;
            }

            // Venom droplets
            if (Main.rand.NextBool(8))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile. height,
                    DustID. Poisoned, 0f, 2f, 100, default, 0.7f);
                Main.dust[dust].noGravity = false;
            }

            // Lighting
            Lighting.AddLight(Projectile.Center, 0.3f, 0.6f, 0.1f);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int difficulty = Main.masterMode ? 2 : (Main.expertMode ?  1 : 0);
            int[] venomDuration = { 120, 180, 240 };
            
            target.AddBuff(BuffID.Venom, venomDuration[difficulty]);

            // Venom splash
            for (int i = 0; i < 12; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Poisoned,
                    velocity.X, velocity.Y, 100, Color.LimeGreen, 1.2f);
                Main.dust[dust].noGravity = Main.rand.NextBool();
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(150, 255, 100, 220);
        }
    }
}