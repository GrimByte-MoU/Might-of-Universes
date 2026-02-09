using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna. Framework;
using System;

namespace MightofUniverses.Content.Items. Projectiles.EnemyProjectiles
{
    public class SkySpark : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile. alpha = 0;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.rotation += 0.3f;
            if (Projectile.velocity.Y > 8f)
                Projectile. velocity.Y = 8f;

            Projectile.velocity.X += (float)Math.Sin(Projectile.ai[0] * 0.1f) * 0.1f;
            Projectile.ai[0]++;

            if (Main.rand. NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile. height,
                    DustID.Electric, 0f, 0f, 100, Color.Yellow, 1.1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
            }
            if (Main. rand.NextBool(10))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile. height,
                    DustID.BlueFairy, 0f, 0f, 150, Color.Cyan, 0.8f);
                Main.dust[dust].noGravity = true;
            }

            float pulse = (float)Math.Sin(Projectile.ai[0] * 0.2f) * 0.3f + 0.7f;
            Lighting.AddLight(Projectile.Center, 0.8f * pulse, 0.8f * pulse, 0.3f * pulse);
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
{
    modifiers.FinalDamage.Base = 100;
}


        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int difficulty = Main.masterMode ? 2 : (Main.expertMode ? 1 : 0);
            int[] electrifiedDuration = { 240, 300, 360 };

            target.AddBuff(BuffID.Electrified, electrifiedDuration[difficulty]);

            for (int i = 0; i < 20; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(5f, 5f);
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Electric,
                    velocity.X, velocity.Y, 100, Color.Yellow, 1.5f);
                Main.dust[dust].noGravity = true;
            }

            for (int i = 0; i < 8; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.BlueFairy,
                    Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f), 150, Color.Cyan, 1.2f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 150, 230);
        }
    }
}