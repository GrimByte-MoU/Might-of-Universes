using System;
using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Buffs;


using EmpVals = MightofUniverses.Common.Players.ReaperEmpowermentValues;

namespace MightofUniverses.Common
{
    public static class ReaperSoulEffects
    {
        public static bool TryReleaseSoulsWithEmpowerment(
            Player player,
            float cost,
            int durationTicks,
            Action<EmpVals> configure,
            Action<Player> onConsume = null,
            string message = null)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            int effectiveCost = (int)Math.Floor(Math.Max(1f, cost));

            var releaseResult = reaper.TryReleaseSouls(
                effectiveCost,
                onSuccess: p =>
                {
                    var vals = new EmpVals();
                    configure?.Invoke(vals);
                    var state = p.GetModPlayer<ReaperEmpowermentState>();
                    state.Values = vals;
                    p.AddBuff(ModContent.BuffType<SoulEmpowerment>(), durationTicks);

                    onConsume?.Invoke(p);
                }
            );

            bool released = releaseResult;

            return released;
        }
    }
}