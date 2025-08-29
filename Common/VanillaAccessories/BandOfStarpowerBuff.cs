using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class BandOfStarpowerBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.BandofStarpower)
            {
                tooltips.Add(new TooltipLine(Mod, "BandofStarpowerBuff1", "+5% magic damage bonus"));
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.BandofStarpower)
            {
                player.GetDamage(DamageClass.Magic) += 0.05f;
            }
        }
    }
}