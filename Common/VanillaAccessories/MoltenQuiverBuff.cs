using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class MoltenQuiverBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.MoltenQuiver)
    {
        tooltips.Add(new TooltipLine(Mod, "MoltenQuiverBuff", "Ranged attacks ignore 15 defense"));
    }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.MoltenQuiver)
    {
        player.GetArmorPenetration(DamageClass.Ranged) += 15;
    }
        }
    }
}