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
        private const string DEATH_EMPTY_PATH = "MightofUniverses/Common/UI/DeathMark_Empty";
        private const string DEATH_FILLED_PATH = "MightofUniverses/Common/UI/DeathMark_Filled";

        private const int FILL_LEFT_PAD_PX = 66;
        private const int FILL_RIGHT_PAD_PX = 10;

        private float displayedSoulPercent = 0f;
        private bool playedMaxSound = false;

        // Death mark visuals
        private const int DeathMaxSlots = 5;
        private Texture2D deathEmptyTex;
        private Texture2D deathFilledTex;
        private float[] deathSlotScale = new float[DeathMaxSlots];
        private int[] deathSlotFlash = new int[DeathMaxSlots];
        private float[] deathGlowAlpha = new float[DeathMaxSlots];
        private int prevDeathMarks = -1;
        private int deathIconSize = 32; // fallback until we read texture size
        private int deathSpacing = 6;
        private int deathPaddingFromBar = 8;

        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            ReaperPlayer reaper = player.GetModPlayer<ReaperPlayer>();

            if (!reaper.hasReaperArmor)
                return;

            Texture2D barTexture = ModContent.Request<Texture2D>(SOUL_BAR_PATH).Value;
            Texture2D fillTexture = ModContent.Request<Texture2D>(SOUL_FILL_PATH).Value;

            // Lazy-load death textures from Common/UI
            if (deathEmptyTex == null)
            {
                try { deathEmptyTex = ModContent.Request<Texture2D>(DEATH_EMPTY_PATH).Value; } catch { deathEmptyTex = null; }
            }
            if (deathFilledTex == null)
            {
                try { deathFilledTex = ModContent.Request<Texture2D>(DEATH_FILLED_PATH).Value; } catch { deathFilledTex = null; }
            }
            if (deathEmptyTex != null)
            {
                deathIconSize = Math.Max(1, Math.Min(deathEmptyTex.Width, deathEmptyTex.Height));
            }
            else if (deathFilledTex != null)
            {
                deathIconSize = Math.Max(1, Math.Min(deathFilledTex.Width, deathFilledTex.Height));
            }

            Vector2 position = new Vector2(500, 80);

            float targetFillPercent = 0f;
            if (reaper.maxSoulEnergy > 0f)
                targetFillPercent = MathHelper.Clamp(reaper.soulEnergy / reaper.maxSoulEnergy, 0f, 1f);

            displayedSoulPercent = MathHelper.Lerp(displayedSoulPercent, targetFillPercent, 0.1f);

            // Draw soul bar and fill
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
                    Rectangle dustRect = new Rectangle((int)(position.X + FILL_LEFT_PAD_PX), (int)position.Y, visibleUsableWidth, fillTexture.Height);
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
                    SoundEngine.PlaySound(SoundID.NPCDeath54 with { Volume = 0.8f }, player.Center);
                    playedMaxSound = true;
                }
            }
            else
            {
                playedMaxSound = false;
            }

            // ----------------------------
            // Draw Death Marks to the right
            // ----------------------------
            int marks = (int)MathHelper.Clamp(reaper.deathMarks, 0f, (float)DeathMaxSlots);
            if (prevDeathMarks == -1)
            {
                prevDeathMarks = marks;
                for (int i = 0; i < marks; i++)
                    deathGlowAlpha[i] = 0.35f;
            }
            else if (marks > prevDeathMarks)
            {
                for (int i = prevDeathMarks; i < marks && i < DeathMaxSlots; i++)
                    TriggerDeathGain(i);
            }
            else if (marks < prevDeathMarks)
            {
                int consumed = prevDeathMarks - marks;
                int start = marks;
                TriggerDeathConsume(start, consumed);
            }
            prevDeathMarks = marks;
            for (int i = 0; i < DeathMaxSlots; i++)
            {
                deathSlotScale[i] = MathHelper.Lerp(deathSlotScale[i], 1f, 0.18f);
                if (deathSlotFlash[i] > 0) deathSlotFlash[i] = Math.Max(0, deathSlotFlash[i] - 1);

                bool filled = i < marks;
                float target = filled ? 0.35f : 0f;
                deathGlowAlpha[i] = MathHelper.Lerp(deathGlowAlpha[i], target, 0.08f);
            }
            Vector2 marksOrigin = position + new Vector2(barTexture.Width + deathPaddingFromBar, (barTexture.Height - deathIconSize) / 2f);
            for (int i = 0; i < DeathMaxSlots; i++)
            {
                Vector2 slotPos = marksOrigin + new Vector2((deathIconSize + deathSpacing) * i, 0f);
                Rectangle dest = new Rectangle((int)slotPos.X, (int)slotPos.Y, deathIconSize, deathIconSize);

                bool filled = i < marks;

                // glow behind the slot (draw larger and with alpha) using filled texture if available
                if (filled && deathGlowAlpha[i] > 0.01f && deathFilledTex != null)
                {
                    float glowScale = 1.35f + (deathSlotScale[i] - 1f) * 0.35f;
                    int gw = (int)(deathIconSize * glowScale);
                    int gh = (int)(deathIconSize * glowScale);
                    Vector2 glowPos = slotPos + new Vector2((deathIconSize - gw) / 2f, (deathIconSize - gh) / 2f);
                    Color glowColor = Color.White * deathGlowAlpha[i];
                    spriteBatch.Draw(deathFilledTex, new Rectangle((int)glowPos.X, (int)glowPos.Y, gw, gh), glowColor);
                }

                Texture2D tex = filled ? deathFilledTex : deathEmptyTex;

                if (tex != null)
                {
                    float scale = deathSlotScale[i];
                    int drawW = (int)(deathIconSize * scale);
                    int drawH = (int)(deathIconSize * scale);
                    Vector2 drawPos = slotPos + new Vector2((deathIconSize - drawW) / 2f, (deathIconSize - drawH) / 2f);

                    Color color = Color.White;
                    if (!filled) color = Color.Lerp(Color.Gray, Color.White, 0.15f);

                    spriteBatch.Draw(tex, new Rectangle((int)drawPos.X, (int)drawPos.Y, drawW, drawH), color);

                    if (deathSlotFlash[i] > 0)
                    {
                        float alpha = deathSlotFlash[i] / 12f;
                        spriteBatch.Draw(tex, dest, Color.White * alpha);
                    }
                }
            }
        }

        private void TriggerDeathGain(int index)
        {
            if (index < 0 || index >= DeathMaxSlots) return;
            deathSlotScale[index] = 1.28f;
            deathSlotFlash[index] = 14;
            deathGlowAlpha[index] = 1.1f;
            SoundEngine.PlaySound(SoundID.Item37, Main.LocalPlayer.position);
        }

        private void TriggerDeathConsume(int startIndex, int count)
        {
            for (int i = startIndex; i < startIndex + count && i < DeathMaxSlots; i++)
            {
                deathSlotFlash[i] = 14;
                deathGlowAlpha[i] = 0.0f;
            }
            SoundEngine.PlaySound(SoundID.Item14, Main.LocalPlayer.position);
        }
    }
}