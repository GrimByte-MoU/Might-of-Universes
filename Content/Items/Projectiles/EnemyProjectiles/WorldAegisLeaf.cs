// Content/Items/Projectiles/EnemyProjectiles/WorldAegisLeaf.cs

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace MightofUniverses.Content.Items.Projectiles.EnemyProjectiles
{
    public class WorldAegisLeaf : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
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

            Projectile.velocity. X += (float)Math.Sin(Projectile.ai[0] * 0.1f) * 0.2f;
            Projectile. ai[0]++;

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile. height,
                    DustID. Grass, 0f, 0f, 100, Color.ForestGreen, 1.0f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.2f;
            }

            Lighting.AddLight(Projectile. Center, 0.3f, 0.6f, 0.3f);
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
{
    modifiers.FinalDamage.Base = Projectile.damage;
}


        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            for (int i = 0; i < 8; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Grass,
                    velocity.X, velocity.Y, 100, Color.ForestGreen, 1.2f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(100, 200, 100, 210);
        }
    }
}