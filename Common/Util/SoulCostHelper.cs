using System;
using Terraria;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.Util
{
    public static class SoulCostHelper
    {
        private static int RoundCost(float v) => (int)Math.Floor(v);

        public static int ComputeEffectiveSoulCostInt(Player player, float baseCost)
        {
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            float cost = baseCost;

            float multi = Math.Max(ReaperAccessoryPlayer.MinEffectiveCostMultiplier, acc.SoulCostMultiplier);
            cost *= multi;

            cost -= acc.SoulCostFlatReduction;

            cost = Math.Max(1f, cost);

            return RoundCost(cost);
        }
    }
}