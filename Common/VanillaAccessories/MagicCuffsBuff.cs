using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class MagicCuffsBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.MagicCuffs)
            {
                tooltips.Add(new TooltipLine(Mod, "MagicCuffsBuff1", "+5% magic damage bonus"));
                tooltips.Add(new TooltipLine(Mod, "MagicCuffsBuff2", "+1% damage reduction"));

            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.MagicCuffs)
            {
                player.GetDamage(DamageClass.Magic) += 0.05f;
                player.endurance += 0.01f;
            }
        }
    }
}