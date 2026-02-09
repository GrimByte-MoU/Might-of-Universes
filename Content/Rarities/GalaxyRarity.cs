using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace MightofUniverses.Content.Rarities
{
   public class GalaxyRarity : ModRarity
    {
        public override Color RarityColor
        {
            get
            {
                float time = Main.GlobalTimeWrappedHourly * 2f;
                Color[] colors = new Color[] {
                    new Color(48, 25, 52),
                    new Color(75, 0, 130),
                    new Color(98, 0, 255)
                };
                int index = (int)(time % colors.Length);
                int nextIndex = (index + 1) % colors.Length;
                float lerp = time % 1f;
                return Color.Lerp(colors[index], colors[nextIndex], lerp);
            }
        }
    }
}
