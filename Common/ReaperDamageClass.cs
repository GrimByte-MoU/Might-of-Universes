using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common
{
    public class ReaperDamageClass : DamageClass
    {
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == DamageClass.Generic)
                return StatInheritanceData.Full;

            if (damageClass is ReaperDamageClass)
                return StatInheritanceData.Full;

            return StatInheritanceData.None;
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return damageClass == DamageClass.Generic;
        }

        public override bool UseStandardCritCalcs => true;

        public override void SetDefaultStats(Player player)
        {
        }

        public override bool ShowStatTooltipLine(Player player, string lineName)
        {
            return true;
        }
    }
}
