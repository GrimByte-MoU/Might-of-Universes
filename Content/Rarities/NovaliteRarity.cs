using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace MightofUniverses.Content.Rarities
{
        public class NovaliteRarity : ModRarity
    {
        public override Color RarityColor
        {
            get
            {
                float time = Main.GlobalTimeWrappedHourly * 1.2f;
                Color[] colors = new Color[]
                {
                    new Color(48, 25, 52),   // Dark Purple
                    new Color(88, 24, 69),   // Dark Magenta
                    new Color(60, 12, 80)    // Deep Magenta
                };
                int currentIndex = (int)(time % colors.Length);
                int nextIndex = (currentIndex + 1) % colors.Length;
                float lerp = time % 1f;
                return Color.Lerp(colors[currentIndex], colors[nextIndex], lerp);
            }
        }
    }
}