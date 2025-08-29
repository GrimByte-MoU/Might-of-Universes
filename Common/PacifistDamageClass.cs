using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Common
{
    public class PacifistDamageClass : DamageClass
    {
        // Prevents Pacifist damage from inheriting any other damage class boosts (generic or class-specific)
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return false;
        }

        // Keeps it isolated from all other damage sources
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            return StatInheritanceData.None;
        }
    }
}

