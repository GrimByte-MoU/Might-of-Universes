using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using MightofUniverses.Common.Players;
using Terraria.ID;
using System;

namespace MightofUniverses.Common.UI
{
    public class ReaperUI : UIState
    {
        private const string SOUL_BAR_PATH = "MightofUniverses/Common/UI/SoulBar";
        private const string SOUL_FILL_PATH = "MightofUniverses/Common/UI/SoulBarFill";
        private const int FILL_LEFT_PAD_PX = 66;
        private const int FILL_RIGHT_PAD_PX = 10;

        private float displayedSoulPercent = 0f;
        private bool playedMaxSound = false;

        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            ReaperPlayer reaper = player.GetModPlayer<ReaperPlayer>();

            if (!reaper.hasReaperArmor)
                return;

            Texture2D barTexture = ModContent.Request<Texture2D>(SOUL_BAR_PATH).Value;
            Texture2D fillTexture = ModContent.Request<Texture2D>(SOUL_FILL_PATH).Value;
            Texture2D px = TextureAssets.MagicPixel.Value;

            Vector2 position = new Vector2(500, 80);

            float targetFillPercent = 0f;
            if (reaper.maxSoulEnergy > 0f)
                targetFillPercent = MathHelper.Clamp(reaper.soulEnergy / reaper.maxSoulEnergy, 0f, 1f);

            displayedSoulPercent = MathHelper.Lerp(displayedSoulPercent, targetFillPercent, 0.1f);

            spriteBatch.Draw(barTexture, position, Color.White);

            int usableWidth = Math.Max(0, fillTexture.Width - FILL_LEFT_PAD_PX - FILL_RIGHT_PAD_PX);

            int visibleUsableWidth = (int)Math.Round(usableWidth * displayedSoulPercent);
            if (visibleUsableWidth > 0)
            {
                Rectangle fillSrc = new Rectangle(FILL_LEFT_PAD_PX, 0, visibleUsableWidth, fillTexture.Height);
                Vector2 fillPos = position + new Vector2(FILL_LEFT_PAD_PX, 0f);
                spriteBatch.Draw(fillTexture, fillPos, fillSrc, Color.White);

                bool isFull = (reaper.maxSoulEnergy > 0f && reaper.soulEnergy >= reaper.maxSoulEnergy - 0.001f)
                              || displayedSoulPercent >= 0.995f;

                if (isFull)
                {
                    Rectangle fillDst = new Rectangle((int)fillPos.X, (int)fillPos.Y, fillSrc.Width, fillSrc.Height);

                    float time = (float)Main.GlobalTimeWrappedHourly;
                    float pulse = 0.5f + 0.5f * (float)Math.Sin(time * 4.0f);
                    float baseGlow = 0.25f;
                    float pulseGlow = 0.35f;
                    float shimmerSpeed = 80f;
                    int shimmerWidth = 14;
                    float shimmerAlpha = 0.55f;
                    var glowColor = new Color(120, 255, 220);

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);

                    float glowA = baseGlow + pulseGlow * pulse;
                    spriteBatch.Draw(fillTexture, fillDst, fillSrc, glowColor * glowA);

                    float sweep = (time * shimmerSpeed) % Math.Max(1, fillDst.Width);
                    int bandLeft = fillDst.Left + (int)sweep - shimmerWidth / 2;
                    Rectangle bandDst = new Rectangle(bandLeft, fillDst.Top, shimmerWidth, fillDst.Height);

                    int srcOffsetX = bandLeft - fillDst.Left;
                    int srcX = FILL_LEFT_PAD_PX + Math.Clamp(srcOffsetX, 0, Math.Max(0, fillSrc.Width - 1));
                    int srcWidth = Math.Max(0, Math.Min(shimmerWidth, fillSrc.Width - Math.Clamp(srcOffsetX, 0, fillSrc.Width)));
                    if (srcWidth > 0)
                    {
                        Rectangle bandSrc = new Rectangle(srcX, 0, srcWidth, fillSrc.Height);

                        Rectangle drawDst = new Rectangle(bandLeft + Math.Max(0, -srcOffsetX), fillDst.Top, srcWidth, fillDst.Height);

                        spriteBatch.Draw(fillTexture, drawDst, bandSrc, glowColor * shimmerAlpha);
                    }

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
                }

                if (reaper.soulEnergy > 0 && Main.rand.NextFloat() < 0.1f)
                {
                    Rectangle dustRect = new Rectangle((int)fillPos.X, (int)fillPos.Y, fillSrc.Width, fillSrc.Height);
                    int dustIndex = Dust.NewDust(dustRect.Location.ToVector2(), dustRect.Width, dustRect.Height, DustID.GreenTorch);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 0.1f;
                }
            }

            string soulText = $"Soul Energy: {(int)reaper.soulEnergy}/{(int)reaper.maxSoulEnergy}";
            Vector2 textSize = FontAssets.MouseText.Value.MeasureString(soulText);
            Vector2 textPosition = position + new Vector2(barTexture.Width / 2f - textSize.X / 2f, -24);
            Utils.DrawBorderString(spriteBatch, soulText, textPosition, Color.White);

            if (reaper.maxSoulEnergy > 0f && reaper.soulEnergy >= reaper.maxSoulEnergy)
            {
                if (!playedMaxSound)
                {
                    SoundEngine.PlaySound(SoundID.Item4 with { Volume = 0.8f }, player.Center);
                    playedMaxSound = true;
                }
            }
            else
            {
                playedMaxSound = false;
            }
        }
    }
}