// Content/Items/Projectiles/EnemyProjectiles/WorldAegisWater.cs

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft. Xna.Framework;
using System;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles.EnemyProjectiles
{
    public class WorldAegisWater : MoUProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SafeSetDefaults()
        {
            Projectile. width = 18;
            Projectile.height = 18;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile. alpha = 50;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            Projectile.velocity.Y += (float)Math.Sin(Projectile.ai[0] * 0.08f) * 0.08f;
            Projectile. ai[0]++;

            if (Main.rand.NextBool(2))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile. height,
                    DustID.Water, 0f, 0f, 100, Color.DeepSkyBlue, 1.1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.25f;
            }

            Lighting.AddLight(Projectile.Center, 0.2f, 0.4f, 0.7f);
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
{
    modifiers.FinalDamage.Base = 95;
}


        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int difficulty = Main.masterMode ? 2 : (Main.expertMode ? 1 : 0);
            int[] drowningDuration = { 60, 120, 180 };

            target.AddBuff(ModContent.BuffType<Drowning>(), drowningDuration[difficulty]);

            for (int i = 0; i < 10; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(3.5f, 3.5f);
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Water,
                    velocity.X, velocity.Y, 100, Color.DeepSkyBlue, 1.3f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(100, 180, 255, 200);
        }
    }
}