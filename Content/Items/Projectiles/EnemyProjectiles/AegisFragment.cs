// Content/Items/Projectiles/EnemyProjectiles/AegisFragment.cs

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items. Buffs;

namespace MightofUniverses.Content.Items.Projectiles.EnemyProjectiles
{
    public class AegisFragment : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile. width = 20;
            Projectile.height = 20;
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
            Projectile.rotation += 0.15f;

            if (Main. rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile. height,
                    DustID.GoldCoin, 0f, 0f, 100, Color.Orange, 1.2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile.Center, 1.0f, 0.7f, 0.3f);
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
{
    modifiers.FinalDamage.Base = Projectile.damage;
}


        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int difficulty = Main.masterMode ? 2 : (Main.expertMode ? 1 : 0);
            int[] terrasRendDuration = { 90, 180, 270 };

            target.AddBuff(ModContent.BuffType<TerrasRend>(), terrasRendDuration[difficulty]);

            for (int i = 0; i < 12; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GoldCoin,
                    velocity.X, velocity.Y, 100, Color.Orange, 1.5f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 200, 80, 230);
        }
    }
}