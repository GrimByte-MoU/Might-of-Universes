using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class FartInABalloonBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.FartInABalloon)
            {
                tooltips.Add(new TooltipLine(Mod, "FartInABalloonBuff1", "+5% movement speed"));
                tooltips.Add(new TooltipLine(Mod, "FartInABalloonBuff2", "Increased fall damage resistance"));
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.FartInABalloon)
            {
                player.moveSpeed += 0.05f;
                player.maxFallSpeed += 12f;
            }
        }
    }
}