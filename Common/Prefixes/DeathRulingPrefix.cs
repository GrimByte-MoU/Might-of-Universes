using Terraria.ModLoader;

namespace MightofUniverses.Common.Prefixes
{
    public class DeathRulingPrefix : ReaperPrefix
    {
        public override float GetDamageMult() => 1.15f;
        public override float GetKnockbackMult() => 1.15f;
        public override float GetUseTimeMult() => 0.85f;
        public override float GetScaleMult() => 1.10f;
        public override int GetCritBonus() => 10;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 2.0f;
        }
    }
}