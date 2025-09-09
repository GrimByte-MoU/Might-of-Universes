using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Common.GlobalItems
{
    public class CactusArmorGlobal : GlobalItem
    {
        public override bool InstancePerEntity => false;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.CactusHelmet
             || item.type == ItemID.CactusBreastplate
             || item.type == ItemID.CactusLeggings)
            {
            }
        }
    }
}