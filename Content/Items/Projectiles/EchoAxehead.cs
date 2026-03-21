using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class EchoAxehead : MoUProjectile
    {
        public override void SafeSetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 120;
            Projectile.alpha = 50;
            Projectile.light = 0.8f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = ModContent.GetInstance<ReaperDamageClass>();
        }

        public override void AI()
        {
            Projectile.rotation += 0.4f;

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.RainbowMk2,
                    0f,
                    0f,
                    100,
                    default,
                    1.5f
                );
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            Lighting.AddLight(Projectile.Center, 0.6f, 0.4f, 0.8f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(5f, target.Center);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    DustID.RainbowMk2,
                    Main.rand.NextFloat(-4f, 4f),
                    Main.rand.NextFloat(-4f, 4f),
                    100,
                    default,
                    Main.rand.NextFloat(1.5f, 2.5f)
                );
                dust.noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 150, 255, 150);
        }
    }
}