using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class OpulencePlayer : ModPlayer
    {
        public bool hasOpulence;

        public override void ResetEffects()
        {
            hasOpulence = false;
        }

        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (hasOpulence)
            {
                // Implement any visual effects here if needed
            }
        }
    }
}
