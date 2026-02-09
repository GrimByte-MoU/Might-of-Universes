using System;
using Terraria.ModLoader;

namespace MightofUniverses.Common.Players
{
    public class SoulCostPlayer : ModPlayer
    {
        public float SoulCostMult;
        public float SoulCostFlat;

        public override void ResetEffects()
        {
            SoulCostMult = 1f;
            SoulCostFlat = 0f;
        }
        public int ComputeEffectiveSoulCostInt(float baseCost)
        {
            float cost = (baseCost - SoulCostFlat) * SoulCostMult;
            cost = MathF.Max(1f, cost);
            return (int)MathF.Ceiling(cost);
        }
    }
}