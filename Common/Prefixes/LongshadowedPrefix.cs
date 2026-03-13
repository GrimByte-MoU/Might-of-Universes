using Terraria.ModLoader;

namespace MightofUniverses.Common.Prefixes
{
    public class LongshadowedPrefix : ReaperPrefix
    {
        public override float GetDamageMult() => 1.25f;
        public override float GetKnockbackMult() => 1.25f;
        public override float GetUseTimeMult() => 1.20f;
        public override float GetScaleMult() => 1.25f;
        public override int GetCritBonus() => 0;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.2f;
        }
    }
}