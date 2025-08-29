using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class FrogFlipperBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.FrogFlipper)
            {
                tooltips.Add(new TooltipLine(Mod, "FrogFlipperBuff1", "+15% movement speed"));
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.FrogLeg)
            {
                player.moveSpeed += 0.15f;
            }
        }
    }
}