using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles.EnemyProjectiles
{
    public class OceanSphere : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
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
            Projectile.rotation += 0.1f;

            // Water bubble visual effect
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Water, 0f, 0f, 100, Color.CornflowerBlue, 1.2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
            }

            // Gentle floating motion
            Projectile.velocity. Y += (float)System.Math.Sin(Projectile.ai[0] * 0.05f) * 0.05f;
            Projectile. ai[0]++;

            // Lighting
            Lighting.AddLight(Projectile. Center, 0.2f, 0.4f, 0.8f);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int difficulty = Main.masterMode ? 2 : (Main.expertMode ? 1 : 0);
            int[] drowningDuration = { 120, 180, 240 };
            
                        target.AddBuff(ModContent.BuffType<Drowning>(), drowningDuration[difficulty]);
            for (int i = 0; i < 15; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Water,
                    velocity.X, velocity.Y, 100, Color.CornflowerBlue, 1.5f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(100, 150, 255, 200);
        }
    }
}