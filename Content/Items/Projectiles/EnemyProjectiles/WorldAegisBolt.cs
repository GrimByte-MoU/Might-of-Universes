// Content/Items/Projectiles/EnemyProjectiles/WorldAegisBolt.cs

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft. Xna.Framework;

namespace MightofUniverses.Content.Items.Projectiles.EnemyProjectiles
{
    public class WorldAegisBolt : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
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
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                    DustID.SilverCoin, 0f, 0f, 100, Color.Silver, 0.9f);
                Main. dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.2f;
            }

            Lighting.AddLight(Projectile.Center, 0.6f, 0.6f, 0.6f);
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
{
    float baseDamage = 95f;
    modifiers.FinalDamage.Base = baseDamage;
}

public override void OnHitPlayer(Player target, Player.HurtInfo info)
{

    for (int i = 0; i < 6; i++)
    {
        Vector2 velocity = Main.rand.NextVector2Circular(2.5f, 2.5f);
        int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.SilverCoin,
            velocity.X, velocity.Y, 100, Color.Silver, 1.1f);
        Main.dust[dust].noGravity = true;
    }
}

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 200, 220, 210);
        }
    }
}