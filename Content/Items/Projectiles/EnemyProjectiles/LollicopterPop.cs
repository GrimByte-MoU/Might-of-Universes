using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles.EnemyProjectiles
{
    public class LollicopterPop : MoUProjectile
    {
        private const float RollSpeed = 5f;
        private const float Gravity = 0.4f;
        private const int StepUpHeight = 16;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 300;
            Projectile.aiStyle = -1;
            Projectile.scale = 2f;
        }

        public override void AI()
        {
            float direction = Projectile.velocity.X >= 0 ? 1f : -1f;
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[0] = direction;
            }

            Projectile.velocity.Y += Gravity;
            if (Projectile.velocity.Y > 16f)
                Projectile.velocity.Y = 16f;

            Projectile.velocity.X = Projectile.ai[0] * RollSpeed;

            Projectile.rotation += Projectile.ai[0] * 0.2f;

            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.TreasureSparkle, Projectile.velocity.X * 0.2f, 0f, 100, default, 0.8f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (oldVelocity.Y != Projectile.velocity.Y && System.Math.Abs(oldVelocity.Y) > 0f)
            {
                Projectile.velocity.X = Projectile.ai[0] * RollSpeed;
            }

            if (oldVelocity.X != Projectile.velocity.X)
            {
                Vector2 stepPos = new Vector2(
                    Projectile.position.X + oldVelocity.X,
                    Projectile.position.Y - StepUpHeight
                );

                if (!Terraria.Collision.SolidCollision(stepPos, Projectile.width, Projectile.height))
                {
                    Projectile.position.Y -= StepUpHeight;
                    Projectile.velocity.X = Projectile.ai[0] * RollSpeed;
                    Projectile.velocity.Y = -1f;
                }
                else
                {
                    Projectile.Kill();
                }
            }

            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<SugarCrash>(), 60);
        }
    }
}