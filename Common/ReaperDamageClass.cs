using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common
{
    public class ReaperDamageClass : DamageClass
    {
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            // Inherit full stats from generic damage class
            if (damageClass == DamageClass.Generic)
                return StatInheritanceData.Full;

            // Inherit crit chance from ReaperDamageClass itself
            if (damageClass is ReaperDamageClass)
                return StatInheritanceData.Full;

            return StatInheritanceData.None;
        }

        public override bool UseStandardCritCalcs => true;

        public override bool ShowStatTooltipLine(Player player, string lineName)
        {
            return true;
        }
    }
}
