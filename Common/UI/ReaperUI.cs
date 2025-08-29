using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using MightofUniverses.Common.Players;
using Terraria.ID;

namespace MightofUniverses.Common.UI
{
    public class ReaperUI : UIState
    {
        private const string SOUL_BAR_PATH = "MightofUniverses/Common/UI/SoulBar";
        private const string SOUL_FILL_PATH = "MightofUniverses/Common/UI/SoulBarFill";

        private float displayedSoulPercent = 0f;
        private bool playedMaxSound = false;

        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            ReaperPlayer reaper = player.GetModPlayer<ReaperPlayer>();

            if (reaper.hasReaperArmor)
            {
                Texture2D barTexture = ModContent.Request<Texture2D>(SOUL_BAR_PATH).Value;
                Texture2D fillTexture = ModContent.Request<Texture2D>(SOUL_FILL_PATH).Value;

                // Position just to the right of the life hearts
                Vector2 position = new Vector2(500, 80); // Adjust as needed

                // Calculate target fill percentage
                float targetFillPercent = MathHelper.Clamp(reaper.soulEnergy / reaper.maxSoulEnergy, 0f, 1f);

                // Smooth the animation
                displayedSoulPercent = MathHelper.Lerp(displayedSoulPercent, targetFillPercent, 0.1f);

                // Draw the background bar
                spriteBatch.Draw(barTexture, position, Color.White);

                // Draw the filled portion (horizontal fill)
                Rectangle fillRect = new Rectangle(0, 0, (int)(fillTexture.Width * displayedSoulPercent), fillTexture.Height);
                spriteBatch.Draw(fillTexture, position, fillRect, Color.White);

                // Draw text above the bar
                string soulText = $"Soul Energy: {(int)reaper.soulEnergy}/{(int)reaper.maxSoulEnergy}";
                Vector2 textSize = FontAssets.MouseText.Value.MeasureString(soulText);
                Vector2 textPosition = position + new Vector2(barTexture.Width / 2f - textSize.X / 2f, -24);
                Utils.DrawBorderString(spriteBatch, soulText, textPosition, Color.White);

                // Green dust effect when energy is non-zero
                if (reaper.soulEnergy > 0 && Main.rand.NextFloat() < 0.1f)
                {
                    int dustIndex = Dust.NewDust(position, fillRect.Width, fillRect.Height, DustID.GreenTorch);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 0.1f;
                }

                // Sound when full (only plays once per full charge)
                if (reaper.soulEnergy >= reaper.maxSoulEnergy)
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
}
