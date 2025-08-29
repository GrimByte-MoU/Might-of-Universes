using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class StalkersQuiverBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.StalkersQuiver)
    {
        tooltips.Add(new TooltipLine(Mod, "Stalker'sQuiverBuff", "Ranged attacks ignore 15 defense"));
    }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
             if (item.type == ItemID.StalkersQuiver)
    {
        player.GetArmorPenetration(DamageClass.Ranged) += 15;
    }
        }
    }
}