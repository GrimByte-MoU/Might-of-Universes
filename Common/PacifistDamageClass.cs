using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common
{
    public class PacifistDamageClass : DamageClass
    {
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return false;
        }
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            return StatInheritanceData.None;
        }
    }
}

