using Terraria;
using Terraria. ModLoader;
using MightofUniverses.Content.Items. Buffs;

namespace MightofUniverses.Common. Players
{
    public class GuidingLightPlayer : ModPlayer
    {
        private float[] _debuffExtraTickAccumulator;

        public override void Initialize()
        {
            _debuffExtraTickAccumulator = new float[Player.MaxBuffs];
        }

        public override void PostUpdateBuffs()
        {
            int guidingLightType = ModContent.BuffType<GuidingLight>();
            if (!Player. HasBuff(guidingLightType))
                return;
            const float ExtraTicksPerSecond = 12f;
            const float ExtraTicksPerFrame = ExtraTicksPerSecond / 60f;

            for (int i = 0; i < Player. MaxBuffs; i++)
            {
                int buffType = Player.buffType[i];
                if (buffType <= 0) continue;

                if (!Main. debuff[buffType]) continue;

                int buffTime = Player. buffTime[i];

                if (buffTime <= 0) continue;

                _debuffExtraTickAccumulator[i] += ExtraTicksPerFrame;

                while (_debuffExtraTickAccumulator[i] >= 1f && Player. buffTime[i] > 0)
                {
                    Player.buffTime[i]--;
                    _debuffExtraTickAccumulator[i] -= 1f;
                }
            }
        }
    }
}