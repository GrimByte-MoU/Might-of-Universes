using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    // Handles precise 15% faster expiration of debuffs while Guiding Light is active.
    public class GuidingLightPlayer : ModPlayer
    {
        // Per-debuff fractional accumulators so each debuff is reduced by 15% independently.
        // Player.MaxBuffs is the correct size for buffType/buffTime arrays.
        private float[] _debuffExtraTickAcc;

        public override void Initialize()
        {
            _debuffExtraTickAcc = new float[Player.MaxBuffs];
        }

        public override void ResetEffects()
        {
            // No reset needed here for the accumulators; keeping them preserves precision across frames.
        }

        public override void PostUpdateBuffs()
        {
            int guidingLightType = ModContent.BuffType<GuidingLight>();
            if (!Player.HasBuff(guidingLightType))
            {
                return;
            }
            const float extraTicksPerSecond = 10.588235f;
            const float extraTicksPerTick = extraTicksPerSecond / 60f;

            for (int i = 0; i < Player.MaxBuffs; i++)
            {
                int type = Player.buffType[i];
                if (type <= 0) continue;
                if (!Main.debuff[type]) continue; // Only touch debuffs
                int time = Player.buffTime[i];

                // Ignore timeless/infinite debuffs (time <= 0 often means disabled countdown)
                if (time <= 0) continue;

                // Accumulate fractional extra countdown per debuff
                _debuffExtraTickAcc[i] += extraTicksPerTick;

                // For each whole extra tick accumulated, subtract 1 additional tick from this debuff's time.
                while (_debuffExtraTickAcc[i] >= 1f && Player.buffTime[i] > 0)
                {
                    Player.buffTime[i]--;
                    _debuffExtraTickAcc[i] -= 1f;
                }
            }
        }
    }
}