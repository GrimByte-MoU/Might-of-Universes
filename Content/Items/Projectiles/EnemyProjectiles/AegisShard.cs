// Content/Items/Projectiles/EnemyProjectiles/AegisShard.cs

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft. Xna.Framework;
using MightofUniverses.Content. Items.Buffs;

namespace MightofUniverses. Content.Items.Projectiles. EnemyProjectiles
{
    public class AegisShard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
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

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile. height,
                    DustID.GoldCoin, 0f, 0f, 100, Color. Gold, 1.0f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.2f;
            }

            Lighting.AddLight(Projectile. Center, 0.8f, 0.6f, 0.2f);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int difficulty = Main.masterMode ? 2 : (Main.expertMode ?  1 : 0);
            int[] terrasRendDuration = { 60, 120, 180 };
            
            target.AddBuff(ModContent.BuffType<TerrasRend>(), terrasRendDuration[difficulty]);

            for (int i = 0; i < 8; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GoldCoin,
                    velocity.X, velocity.Y, 100, Color.Gold, 1.3f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 215, 100, 220);
        }
    }
}