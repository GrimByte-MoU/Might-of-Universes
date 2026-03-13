using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common;

namespace MightofUniverses.Common.Prefixes
{
    public abstract class ReaperPrefix : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Melee;

        public override bool CanRoll(Item item)
        {
            return item.ModItem != null && item.ModItem is IScytheWeapon;
        }

        public abstract float GetDamageMult();
        public abstract float GetKnockbackMult();
        public abstract float GetUseTimeMult();
        public abstract float GetScaleMult();
        public abstract int GetCritBonus();

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = GetDamageMult();
            knockbackMult = GetKnockbackMult();
            useTimeMult = GetUseTimeMult();
            scaleMult = GetScaleMult();
            critBonus = GetCritBonus();
        }
    }
}