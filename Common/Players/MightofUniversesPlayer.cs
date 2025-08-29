using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class MightofUniversesPlayer : ModPlayer
    {
        public float accessoryDamageMultiplier = 1f;

        public override void ResetEffects()
        {
            accessoryDamageMultiplier = 1f;
        }
    }
}
