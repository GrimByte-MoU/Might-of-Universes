using System;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class SoulCostPlayer : ModPlayer
    {
        // Multiplicative and flat reductions that reset each tick.
        // Example: 20% cheaper and -5 flat => (base - 5) * 0.8
        public float SoulCostMult;
        public float SoulCostFlat;

        public override void ResetEffects()
        {
            SoulCostMult = 1f;
            SoulCostFlat = 0f;
        }

        // Single source of truth for effective cost
        public int ComputeEffectiveSoulCostInt(float baseCost)
        {
            float cost = (baseCost - SoulCostFlat) * SoulCostMult;
            // clamp to at least 1, then round up to an int
            cost = MathF.Max(1f, cost);
            return (int)MathF.Ceiling(cost);
        }
    }
}