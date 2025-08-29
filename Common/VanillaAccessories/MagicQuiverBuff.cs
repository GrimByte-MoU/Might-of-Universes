using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class MagicQuiverBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.MagicQuiver)
    {
        tooltips.Add(new TooltipLine(Mod, "MagicQuiverBuff", "Ranged attacks ignore 10 defense"));
    }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.MagicQuiver)
    {
        player.GetArmorPenetration(DamageClass.Ranged) += 10;
    }
        }
    }
}