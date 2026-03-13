using Terraria.ModLoader;

namespace MightofUniverses.Common.Prefixes
{
    public class ReapingPrefix : ReaperPrefix
    {
        public override float GetDamageMult() => 1.15f;
        public override float GetKnockbackMult() => 1f;
        public override float GetUseTimeMult() => 0.90f;
        public override float GetScaleMult() => 1f;
        public override int GetCritBonus() => 0;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.4f;
        }
    }
}