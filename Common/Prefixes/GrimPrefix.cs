using Terraria.ModLoader;

namespace MightofUniverses.Common.Prefixes
{
    public class GrimPrefix : ReaperPrefix
    {
        public override float GetDamageMult() => 1.10f;
        public override float GetKnockbackMult() => 1f;
        public override float GetUseTimeMult() => 1f;
        public override float GetScaleMult() => 1f;
        public override int GetCritBonus() => 25;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.5f;
        }
    }
}