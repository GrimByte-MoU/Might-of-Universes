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
            Projectile.alpha = 0;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            // Visual leaf sway only (does not affect flight path)
            Projectile.ai[0]++;
            float sway = (float)Math.Sin(Projectile.ai[0] * 0.15f) * 0.15f;
            Projectile.rotation = Projectile.velocity.ToRotation() + sway;

            // Swaying particle trail (optional, visual only)
            if (Main.rand.NextBool(3))
            {
                float radius = 8f;
                Vector2 effectOffset = new Vector2((float)Math.Sin(Projectile.ai[0] * 0.1f) * radius, 0f)
                    .RotatedBy(Projectile.velocity.ToRotation());
                int dust = Dust.NewDust(Projectile.Center + effectOffset, 0, 0, DustID.Grass,
                    0f, 0f, 100, Color.ForestGreen, 1.0f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.2f;
            }

            Lighting.AddLight(Projectile.Center, 0.3f, 0.6f, 0.3f);
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.FinalDamage.Base = 100;
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