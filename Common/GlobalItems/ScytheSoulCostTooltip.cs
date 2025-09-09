using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Common.GlobalItems
{
    public class ScytheSoulCostTooltip : GlobalItem
    {
        public override bool InstancePerEntity => false;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.ModItem is IHasSoulCost hasCost)
            {
                var player = Main.LocalPlayer;
                int effective = SoulCostHelper.ComputeEffectiveSoulCostInt(player, hasCost.BaseSoulCost);
                string line = Language.GetTextValue("Mods.MightofUniverses.Common.Tooltips.ConsumeSouls", effective);
                tooltips.Add(new TooltipLine(Mod, "ConsumeSouls", line));
            }
        }
    }
}