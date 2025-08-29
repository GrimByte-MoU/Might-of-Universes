using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class BSCUPlayer : ModPlayer
    {
        public bool hasBSCU;

        public override void ResetEffects()
        {
            hasBSCU = false;
        }
    }
}
