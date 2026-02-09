// Content/Items/Projectiles/EnemyProjectiles/WorldAegisFireball.cs

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles.EnemyProjectiles
{
    public class WorldAegisFireball : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
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
            Projectile.rotation += 0.15f;

            if (Main. rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID. Torch, 0f, 0f, 100, Color.OrangeRed, 1.3f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
            }

            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Smoke, 0f, 0f, 150, Color.Gray, 0.8f);
                Main. dust[dust].noGravity = false;
            }

            Lighting.AddLight(Projectile.Center, 1.0f, 0.5f, 0.1f);
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
{
    modifiers.FinalDamage.Base = 95;
}


        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int difficulty = Main.masterMode ? 2 : (Main.expertMode ? 1 : 0);
            int[] hellfireDuration = { 120, 180, 240 };

            target.AddBuff(BuffID.OnFire3, hellfireDuration[difficulty]);

            for (int i = 0; i < 15; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch,
                    velocity.X, velocity.Y, 100, Color.OrangeRed, 1.5f);
                Main.dust[dust].noGravity = true;
            }

            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke,
                    Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 150, Color.Gray, 1.2f);
                Main.dust[dust].noGravity = false;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 150, 50, 220);
        }
    }
}