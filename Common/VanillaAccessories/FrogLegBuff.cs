using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class FrogLegBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.FrogLeg)
            {
                tooltips.Add(new TooltipLine(Mod, "FrogLegBuff1", "+10% movement speed"));
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.FrogLeg)
            {
                player.moveSpeed += 0.1f;
            }
        }
    }
}