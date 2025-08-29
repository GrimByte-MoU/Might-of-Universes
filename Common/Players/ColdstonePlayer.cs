using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.Players
{
    public class ColdstonePlayer : ModPlayer
    {
        public bool hasColdstone;

        public override void ResetEffects()
        {
            hasColdstone = false;
        }
    }
}
