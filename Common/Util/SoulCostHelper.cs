using System;
using Terraria;
using MightofUniverses.Common;

namespace MightofUniverses.Common.Util
{
    public static class SoulCostHelper
    {
        // Choose ONE rounding mode for both display and actual consumption.
        // Floor ensures you never overstate the cost in the tooltip.
        private static int RoundCost(float value) => (int)Math.Floor(value);

        // Computes the effective soul cost given a base cost and the player's modifiers.
        // Modifiers come from ReaperAccessoryPlayer (defined below).
        public static int ComputeEffectiveSoulCostInt(Player player, float baseCost)
        {
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            float cost = baseCost;

            // Apply percent multiplier first
            cost *= acc.SoulCostMultiplier;

            // Apply flat subtraction next
            cost -= acc.SoulCostFlatReduction;

            // Clamp to minimum 1
            cost = Math.Max(cost, 1f);

            // Apply unified rounding for both display and spend
            return RoundCost(cost);
        }
    }
}