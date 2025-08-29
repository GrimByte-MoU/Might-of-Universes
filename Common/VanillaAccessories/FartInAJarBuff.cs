using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class FartInABottleBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.FartinaJar)
            {
                tooltips.Add(new TooltipLine(Mod, "FartinaJarBuff1", "+5% movement speed"));
                tooltips.Add(new TooltipLine(Mod, "FartinaJarBuff2", "Increased fall damage resistance"));
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.FartinaJar)
            {
                player.moveSpeed += 0.05f;
                player.maxFallSpeed += 9f;
            }
        }
    }
}