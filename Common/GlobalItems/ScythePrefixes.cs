using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Prefixes;

namespace MightofUniverses.Common.GlobalItems
{
    public class ScytheGlobalItem : GlobalItem
    {
        public override bool AllowPrefix(Item item, int pre)
        {
            if (item.ModItem != null && item.ModItem is IScytheWeapon)
            {
                if (IsVanillaMeleePrefix(pre))
                    return true;

                ModPrefix modPrefix = PrefixLoader.GetPrefix(pre);
                if (modPrefix != null && modPrefix is ReaperPrefix)
                    return true;
            }

            return base.AllowPrefix(item, pre);
        }

        private bool IsVanillaMeleePrefix(int prefixID)
        {
            if (prefixID >= PrefixID.Keen && prefixID <= PrefixID.Broken)
                return true;

            if (prefixID >= PrefixID.Large && prefixID <= PrefixID.Terrible)
                return true;

            switch (prefixID)
            {
                case PrefixID.Legendary:
                case PrefixID.Godly:
                case PrefixID.Demonic:
                case PrefixID.Ruthless:
                case PrefixID.Unreal:
                    return true;
            }

            return false;
        }
    }
}