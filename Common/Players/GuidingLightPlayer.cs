using Terraria;
using Terraria. ModLoader;
using MightofUniverses.Content.Items. Buffs;

namespace MightofUniverses.Common. Players
{
    /// <summary>
    /// Handles Guiding Light's debuff reduction effect: debuffs expire 20% faster.
    /// Uses per-debuff fractional accumulators for precise timing.
    /// </summary>
    public class GuidingLightPlayer : ModPlayer
    {
        private float[] _debuffExtraTickAccumulator;

        public override void Initialize()
        {
            _debuffExtraTickAccumulator = new float[Player.MaxBuffs];
        }

        public override void PostUpdateBuffs()
        {
            // Only active while Guiding Light buff is present
            int guidingLightType = ModContent.BuffType<GuidingLight>();
            if (!Player. HasBuff(guidingLightType))
                return;

            // 20% faster expiration = 12 extra ticks per second (60 * 0.20 = 12)
            const float ExtraTicksPerSecond = 12f;
            const float ExtraTicksPerFrame = ExtraTicksPerSecond / 60f;

            for (int i = 0; i < Player. MaxBuffs; i++)
            {
                int buffType = Player.buffType[i];
                if (buffType <= 0) continue;

                // Only affect debuffs
                if (!Main. debuff[buffType]) continue;

                int buffTime = Player. buffTime[i];

                // Ignore infinite/disabled debuffs (time <= 0)
                if (buffTime <= 0) continue;

                // Accumulate fractional extra ticks
                _debuffExtraTickAccumulator[i] += ExtraTicksPerFrame;

                // Apply whole ticks
                while (_debuffExtraTickAccumulator[i] >= 1f && Player. buffTime[i] > 0)
                {
                    Player.buffTime[i]--;
                    _debuffExtraTickAccumulator[i] -= 1f;
                }
            }
        }
    }
}