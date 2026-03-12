using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Projectiles
{
    public class ChainedSoulProj : ModProjectile
    {
        private const int SoulsGranted = 7;
        private const int HealAmount = 12;
        private const int BuffDuration = 240;

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 50;
            Projectile.scale = 1.1f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            Vector2 direction = player.Center - Projectile.Center;
            float distance = direction.Length();

            if (distance < 30f)
            {
                var reaper = player.GetModPlayer<ReaperPlayer>();
                reaper.AddSoulEnergy(SoulsGranted, Projectile.Center);

                player.Heal(HealAmount);
                player.AddBuff(ModContent.BuffType<CagedSoulBuff>(), BuffDuration);

                for (int i = 0; i < 15; i++)
                {
                    Dust dust = Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.YellowTorch,
                        0f, 0f,
                        100,
                        Color.Gold,
                        1.5f
                    );
                    dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
                    dust.noGravity = true;
                }

                Projectile.Kill();
                return;
            }

            direction.Normalize();
            float speed = 12f + (1f - (distance / 300f)) * 8f;
            Projectile.velocity = direction * speed;

            Projectile.rotation += 0.2f;

            Lighting.AddLight(Projectile.Center, 0.8f, 0.7f, 0.2f);

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.YellowTorch,
                    0f, 0f,
                    100,
                    Color.Gold,
                    1.0f
                );
                dust.velocity *= 0.3f;
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(10))
            {
                int chain = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Iron,
                    0f, 0f,
                    100,
                    Color.Silver,
                    0.8f
                );
                Main.dust[chain].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Color glowColor = Color.Lerp(Color.Gold, Color.White, 0.5f) * (1f - Projectile.alpha / 255f);

            for (int i = 0; i < 3; i++)
            {
                Vector2 offset = new Vector2(2, 0).RotatedBy(MathHelper.TwoPi / 3f * i + Projectile.rotation);
                Main.EntitySpriteDraw(
                    texture,
                    drawPos + offset,
                    null,
                    glowColor * 0.5f,
                    Projectile.rotation,
                    drawOrigin,
                    Projectile.scale * 1.2f,
                    SpriteEffects.None,
                    0
                );
            }

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                Color.White * (1f - Projectile.alpha / 255f),
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}