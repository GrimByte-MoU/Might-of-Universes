using Terraria.ModLoader;

namespace MightofUniverses.Common.Prefixes
{
    public class SatiatedPrefix : ReaperPrefix
    {
        public override float GetDamageMult() => 1f;
        public override float GetKnockbackMult() => 1.30f;
        public override float GetUseTimeMult() => 1f;
        public override float GetScaleMult() => 1.30f;
        public override int GetCritBonus() => 0;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 0.8f;
        }
    }
}