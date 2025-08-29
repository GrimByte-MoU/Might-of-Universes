using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class BundleOfHorseshoeBalloonsBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.HorseshoeBundle)
            {
                tooltips.Add(new TooltipLine(Mod, "HorseshoeBundleBuff1", "+5% movement speed"));
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.HorseshoeBundle)
            {
                player.moveSpeed += 0.05f;
            }
        }
    }
}