using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Rarities
{
    public class CometiniteRarity : ModRarity
    {
        public override Color RarityColor
        {
            get
            {
                float time = Main.GlobalTimeWrappedHourly * 2f;
                Color[] colors = new Color[] {
                    new Color(0, 0, 0),   // Black
                    new Color(0, 0, 139)  // Dark Blue
                };
                int index = (int)(time % colors.Length);
                int nextIndex = (index + 1) % colors.Length;
                float lerp = time % 1f;
                return Color.Lerp(colors[index], colors[nextIndex], lerp);
            }
        }
    }
}