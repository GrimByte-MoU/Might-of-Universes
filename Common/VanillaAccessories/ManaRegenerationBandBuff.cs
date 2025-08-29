using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class ManaRegenerationBandBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.ManaRegenerationBand)
            {
                tooltips.Add(new TooltipLine(Mod, "ManaRegenerationBandBuff1", "+5% magic damage bonus"));
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.ManaRegenerationBand)
            {
                player.GetDamage(DamageClass.Magic) += 0.05f;
            }
        }
    }
}