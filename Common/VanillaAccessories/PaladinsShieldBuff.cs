using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class PaladinsShieldBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.PaladinsShield)
            {
                tooltips.Add(new TooltipLine(Mod, "PaladinsShieldBuff1", "5% chance when damaged to heal for 50% of the damage dealt"));
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.PaladinsShield)
            {
                player.GetModPlayer<DamageHealingPlayer>().hasHealOnHitAccessory = true;
            }
        }
    }
}