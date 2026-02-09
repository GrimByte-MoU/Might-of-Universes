using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace MightofUniverses.Content.Rarities
{
    public class EldritchRarity : ModRarity
    {
        public override Color RarityColor
        {
            get
            {
                float time = Main.GlobalTimeWrappedHourly * 2f;
                Color[] colors = new Color[] {
                    Color.Black,
                    new Color(0, 0, 139),
                    new Color(0, 100, 0)
                };
                int index = (int)(time % colors.Length);
                int nextIndex = (index + 1) % colors.Length;
                float lerp = time % 1f;
                return Color.Lerp(colors[index], colors[nextIndex], lerp);
            }
        }
    }
}

