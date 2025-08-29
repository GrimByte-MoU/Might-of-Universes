using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.Players
{
    public class LavastonePlayer : ModPlayer
    {
        public bool hasLavastone;

        public override void ResetEffects()
        {
            hasLavastone = false;
        }
    }
}