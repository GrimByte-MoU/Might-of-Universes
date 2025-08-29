using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class ShinyStoneBuff : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.ShinyStone)
            {
                player.GetModPlayer<ShinyStoneBuffPlayer>().hasShinyStone = true;
            }
        }

        public override bool AppliesToEntity(Item entity, bool lateInstatiation)
        {
            return true;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.ShinyStone)
            {
                tooltips.Add(new TooltipLine(Mod, "ShinyStoneBuff1", "+50 max health"));
                tooltips.Add(new TooltipLine(Mod, "ShinyStoneBuff2", "Surrounds you with an inferno aura"));
                tooltips.Add(new TooltipLine(Mod, "ShinyStoneBuff3", "-10% movement speed"));
            }
        }
    }
}

