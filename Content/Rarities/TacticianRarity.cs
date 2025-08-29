using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace MightofUniverses.Content.Rarities
{
    public class TacticianRarity : ModRarity
    {
        public override Color RarityColor
        {
            get
            {
                float time = Main.GameUpdateCount * 0.05f;
                Color[] colors = new Color[] {
                    new Color(0, 0, 0),         // Black
                    new Color(139, 0, 0),       // Dark Red
                    new Color(255, 255, 255),   // White
                };
                int currentIndex = (int)(time % colors.Length);
                int nextIndex = (currentIndex + 1) % colors.Length;
                float lerp = time % 1;
                return Color.Lerp(
                    Color.Lerp(colors[currentIndex], colors[nextIndex], lerp),
                    colors[(nextIndex + 1) % colors.Length],
                    (float)Math.Sin(time * 0.5f) * 0.5f + 0.5f
                );
            }
        }
    }
}

