// Content/Items/Projectiles/EnemyProjectiles/AegisChunk.cs

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles.EnemyProjectiles
{
    public class AegisChunk : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
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
            Projectile.rotation += 0.2f;

            if (Main. rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID. GoldCoin, 0f, 0f, 100, Color.DarkGoldenrod, 1.4f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.4f;
            }

            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile. height,
                    DustID.AmberBolt, 0f, 0f, 150, default, 1.0f);
                Main.dust[dust].noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 1.2f, 0.8f, 0.3f);
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
{
    modifiers.FinalDamage.Base = Projectile.damage;
}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {

            int difficulty = Main.masterMode ? 2 : (Main.expertMode ? 1 : 0);
            int[] terrasRendDuration = { 120, 240, 360 };

            target.AddBuff(ModContent.BuffType<TerrasRend>(), terrasRendDuration[difficulty]);

            for (int i = 0; i < 16; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(5f, 5f);
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GoldCoin,
                    velocity.X, velocity.Y, 100, Color.DarkGoldenrod, 1.8f);
                Main.dust[dust].noGravity = true;
            }

            for (int i = 0; i < 8; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.AmberBolt,
                    Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 150, default, 1.3f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 180, 60, 240);
        }
    }
}