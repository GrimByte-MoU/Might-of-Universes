using Terraria.ModLoader;

namespace MightofUniverses.Common.Prefixes
{
    public class RavenousPrefix : ReaperPrefix
    {
        public override float GetDamageMult() => 1.05f;
        public override float GetKnockbackMult() => 1f;
        public override float GetUseTimeMult() => 0.80f;
        public override float GetScaleMult() => 1f;
        public override int GetCritBonus() => 0;

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.3f;
        }
    }
}