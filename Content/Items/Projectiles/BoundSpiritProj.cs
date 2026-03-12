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
    public class BoundSpiritProj : ModProjectile
    {
        private const int SoulsGranted = 9;
        private const int HealAmount = 15;
        private const int BuffDuration = 300;

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 40;
            Projectile.scale = 1.2f;
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

                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.TerraBlade,
                        0f, 0f,
                        100,
                        Color.LimeGreen,
                        2.0f
                    );
                    dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
                    dust.noGravity = true;
                }

                Projectile.Kill();
                return;
            }

            direction.Normalize();
            float speed = 14f + (1f - (distance / 300f)) * 10f;
            Projectile.velocity = direction * speed;

            Projectile.rotation += 0.25f;

            Lighting.AddLight(Projectile.Center, 0.3f, 0.9f, 0.5f);

            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.TerraBlade,
                    0f, 0f,
                    100,
                    Color.LimeGreen,
                    1.2f
                );
                dust.velocity *= 0.3f;
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(8))
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust ring = Dust.NewDustPerfect(
                        Projectile.Center,
                        DustID.JunglePlants,
                        Main.rand.NextVector2Circular(2f, 2f),
                        100,
                        Color.Green,
                        1.5f
                    );
                    ring.noGravity = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Color glowColor = Color.Lerp(Color.LimeGreen, Color.White, 0.6f) * (1f - Projectile.alpha / 255f);

            for (int i = 0; i < 4; i++)
            {
                Vector2 offset = new Vector2(3, 0).RotatedBy(MathHelper.TwoPi / 4f * i + Projectile.rotation);
                Main.EntitySpriteDraw(
                    texture,
                    drawPos + offset,
                    null,
                    glowColor * 0.6f,
                    Projectile.rotation,
                    drawOrigin,
                    Projectile.scale * 1.3f,
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