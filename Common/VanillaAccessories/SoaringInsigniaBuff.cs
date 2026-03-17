using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;

namespace MightofUniverses.Common.VanillaAccessories
{
    public class SoaringInsigniaBuff : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.EmpressFlightBooster)
            {
                player.GetDamage(ModContent.GetInstance<PacifistDamageClass>()) += 0.50f;
            }
        }

        public override void ModifyTooltips(Item item, System.Collections.Generic.List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.EmpressFlightBooster)
            {
                TooltipLine line = new TooltipLine(Mod, "SoaringInsigniaPacifist", "+50% nonweapon damage");
                tooltips.Add(line);
            }
        }
    }
}