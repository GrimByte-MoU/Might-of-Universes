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
        private const int FILL_LEFT_PAD_PX = 66;  // TODO: set to your texture’s actual value
        private const int FILL_RIGHT_PAD_PX = 10; // TODO: set to your texture’s actual value

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

            // Position just to the right of the life hearts (adjust as needed)
            Vector2 position = new Vector2(500, 80);

            // Calculate target fill percentage safely
            float targetFillPercent = 0f;
            if (reaper.maxSoulEnergy > 0f)
                targetFillPercent = MathHelper.Clamp(reaper.soulEnergy / reaper.maxSoulEnergy, 0f, 1f);

            // Smooth the animation
            displayedSoulPercent = MathHelper.Lerp(displayedSoulPercent, targetFillPercent, 0.1f);

            // Draw the background/frame bar
            spriteBatch.Draw(barTexture, position, Color.White);

            // Compute usable interior width (exclude left/right padding)
            int usableWidth = Math.Max(0, fillTexture.Width - FILL_LEFT_PAD_PX - FILL_RIGHT_PAD_PX);

            // Draw the filled portion mapped only across the usable interior
            int visibleUsableWidth = (int)Math.Round(usableWidth * displayedSoulPercent);
            if (visibleUsableWidth > 0)
            {
                Rectangle fillSrc = new Rectangle(FILL_LEFT_PAD_PX, 0, visibleUsableWidth, fillTexture.Height);
                Vector2 fillPos = position + new Vector2(FILL_LEFT_PAD_PX, 0f);
                spriteBatch.Draw(fillTexture, fillPos, fillSrc, Color.White);

                // FULL-CHARGE SHIMMER OVERLAY (additive)
                // Gate by either true full or “visually full” to ensure user sees it.
                bool isFull = (reaper.maxSoulEnergy > 0f && reaper.soulEnergy >= reaper.maxSoulEnergy - 0.001f)
                              || displayedSoulPercent >= 0.995f;

                if (isFull)
                {
                    // Destination rectangle for the current visible interior fill
                    Rectangle fillDst = new Rectangle((int)fillPos.X, (int)fillPos.Y, fillSrc.Width, fillSrc.Height);

                    // Parameters you can tweak
                    float time = (float)Main.GlobalTimeWrappedHourly;
                    float pulse = 0.5f + 0.5f * (float)Math.Sin(time * 4.0f);  // pulse speed
                    float baseGlow = 0.25f;    // baseline glow
                    float pulseGlow = 0.35f;   // pulsing additional glow
                    float shimmerSpeed = 80f;  // pixels per second across the bar
                    int shimmerWidth = 14;     // width of the moving highlight band in pixels
                    float shimmerAlpha = 0.55f; // band intensity
                    var glowColor = new Color(120, 255, 220); // spectral green

                    // Switch to additive for glow effects
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);

                    // 1) Soft whole-bar pulsing glow over the visible interior fill
                    float glowA = baseGlow + pulseGlow * pulse;
                    Rectangle glowRect = new Rectangle(fillDst.X - 2, fillDst.Y - 2, fillDst.Width + 4, fillDst.Height + 4);
                    spriteBatch.Draw(px, glowRect, glowColor * glowA);

                    // 2) Shimmer band that sweeps left->right across the filled region
                    float sweep = (time * shimmerSpeed) % Math.Max(1, fillDst.Width); // 0..width loop
                    int bandLeft = fillDst.Left + (int)sweep - shimmerWidth / 2;
                    int bandRight = bandLeft + shimmerWidth;

                    // Draw the band with a simple horizontal gradient (center brighter)
                    for (int x = bandLeft; x < bandRight; x++)
                    {
                        if (x < fillDst.Left || x >= fillDst.Right) continue;
                        float t = (x - bandLeft) / (float)shimmerWidth;     // 0..1 across band
                        float falloff = 1f - Math.Abs(t * 2f - 1f);          // triangle 0..1..0
                        Color c = glowColor * (shimmerAlpha * falloff);
                        spriteBatch.Draw(px, new Rectangle(x, fillDst.Top, 1, fillDst.Height), c);
                    }

                    // Restore normal alpha blending for the rest of UI
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.UIScaleMatrix);
                }

                // Green dust effect along the visible interior
                if (reaper.soulEnergy > 0 && Main.rand.NextFloat() < 0.1f)
                {
                    Rectangle dustRect = new Rectangle((int)fillPos.X, (int)fillPos.Y, fillSrc.Width, fillSrc.Height);
                    int dustIndex = Dust.NewDust(dustRect.Location.ToVector2(), dustRect.Width, dustRect.Height, DustID.GreenTorch);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 0.1f;
                }
            }

            // Draw text above the bar
            string soulText = $"Soul Energy: {(int)reaper.soulEnergy}/{(int)reaper.maxSoulEnergy}";
            Vector2 textSize = FontAssets.MouseText.Value.MeasureString(soulText);
            Vector2 textPosition = position + new Vector2(barTexture.Width / 2f - textSize.X / 2f, -24);
            Utils.DrawBorderString(spriteBatch, soulText, textPosition, Color.White);

            // Sound when full (only plays once per full charge)
            if (reaper.maxSoulEnergy > 0f && reaper.soulEnergy >= reaper.maxSoulEnergy)
            {
                if (!playedMaxSound)
                {
                    SoundEngine.PlaySound(SoundID.Item4 with { Volume = 0.8f }, player.Center); // Subtle warp/charge sound
                    playedMaxSound = true;
                }
            }
            else
            {
                playedMaxSound = false; // Reset so it plays again next time it hits full
            }
        }
    }
}
