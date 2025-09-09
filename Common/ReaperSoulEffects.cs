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
        // Returns true if souls were consumed and empowerment applied.
        // configure uses the canonical Players.ReaperEmpowermentValues type.
        public static bool TryReleaseSoulsWithEmpowerment(
            Player player,
            float cost,
            int durationTicks,
            Action<EmpVals> configure,
            Action<Player> onConsume = null,
            string message = null)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            // Minimal effective cost (no external dependency) to avoid errors.
            int effectiveCost = (int)Math.Floor(Math.Max(1f, cost));

            // Assumes your ReaperPlayer.TryReleaseSouls returns bool and accepts onSuccess + optional message.
            bool released = reaper.TryReleaseSouls(
                effectiveCost,
                onSuccess: p =>
                {
                    var vals = new EmpVals();
                    configure?.Invoke(vals);
                    var state = p.GetModPlayer<ReaperEmpowermentState>();
                    state.Values = vals;
                    p.AddBuff(ModContent.BuffType<SoulEmpowerment>(), durationTicks);

                    onConsume?.Invoke(p);
                },
                releaseMessage: message
            );

            return released;
        }
    }
}