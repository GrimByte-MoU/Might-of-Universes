using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class CritDamagePlayer : ModPlayer
    {
        public float bonusCritMultiplier = 0f;

        public override void ResetEffects()
        {
            bonusCritMultiplier = 0f;
        }
    }
}
