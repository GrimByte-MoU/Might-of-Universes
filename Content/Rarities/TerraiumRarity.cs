using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace MightofUniverses.Content.Rarities
{
    public class TerraiumRarity : ModRarity
    {
        public override Color RarityColor
        {
            get
            {
                float time = Main.GlobalTimeWrappedHourly * 2f;
                Color[] colors = new Color[] {
                    new Color(85, 107, 47),
                    new Color(139, 69, 19)
                };
                int index = (int)(time % colors.Length);
                int nextIndex = (index + 1) % colors.Length;
                float lerp = time % 1f;
                return Color.Lerp(colors[index], colors[nextIndex], lerp);
            }
        }
    }
}
