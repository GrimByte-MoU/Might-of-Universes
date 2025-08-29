using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class HeroShieldBuff : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.HeroShield)
            {
                tooltips.Add(new TooltipLine(Mod, "HeroShieldBuff1", "5% chance when damaged to heal for 50% of the damage dealt"));
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.HeroShield)
            {
                player.GetModPlayer<DamageHealingPlayer>().hasHealOnHitAccessory = true;
            }
        }
    }
}