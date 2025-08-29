using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class RifleScopeBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.RifleScope)
            {
                tooltips.Add(new TooltipLine(Mod, "RifleScopeBuff1", "+5% ranged crit chance"));
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.RifleScope)
            {
                player.GetCritChance(DamageClass.Ranged) += 0.05f;
            }
        }
    }
}