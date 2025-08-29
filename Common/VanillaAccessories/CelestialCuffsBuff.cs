using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class CelestialCuffsBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.CelestialCuffs)
            {
                tooltips.Add(new TooltipLine(Mod, "MagicCuffsBuff1", "+5% magic damage bonus"));
                tooltips.Add(new TooltipLine(Mod, "MagicCuffsBuff2", "+2% damage reduction"));

            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.CelestialCuffs)
            {
                player.GetDamage(DamageClass.Magic) += 0.05f;
                player.endurance += 0.02f;
            }
        }
    }
}