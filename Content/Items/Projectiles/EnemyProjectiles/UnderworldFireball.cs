using Microsoft. Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Projectiles.EnemyProjectiles
{
    public class UnderworldFireball : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile. friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.alpha = 0;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 1f)
            {
                Player target = Main.player[Player. FindClosest(Projectile. position, Projectile.width, Projectile.height)];
                Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * Projectile.velocity.Length(), 0.01f);
            }

            Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0.1f);

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID. Torch,
                    0f, 0f, 100,
                    default(Color),
                    1.2f
                );
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }

            if (Main.rand. NextBool(4))
            {
                Dust. NewDust(
                    Projectile. position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Smoke,
                    0f, 0f, 100,
                    default(Color),
                    0.8f
                );
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int hellfireDuration = 180;

            if (Main.expertMode)
                hellfireDuration = 240;

            if (Main.masterMode)
                hellfireDuration = 300;

            target.AddBuff(BuffID.OnFire3, hellfireDuration);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID. Torch,
                    Main.rand.NextFloat(-5f, 5f),
                    Main.rand.NextFloat(-5f, 5f),
                    100, default(Color), 1.5f
                );
                dust.noGravity = true;
            }
            for (int i = 0; i < 15; i++)
            {
                Dust. NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Smoke,
                    Main.rand. NextFloat(-3f, 3f),
                    Main.rand.NextFloat(-3f, 3f),
                    100, default(Color), 1.2f
                );
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}