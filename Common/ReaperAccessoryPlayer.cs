using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common
{
    // Provides soul cost modifiers that accessories/armor/buffs can set each tick.
    public class ReaperAccessoryPlayer : ModPlayer
    {
        // Start at no change each tick
        public float SoulCostMultiplier { get; set; }
        public float SoulCostFlatReduction { get; set; }

        public override void ResetEffects()
        {
            SoulCostMultiplier = 1f;
            SoulCostFlatReduction = 0f;
        }
    }
}