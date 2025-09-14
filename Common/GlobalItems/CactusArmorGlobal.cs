using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common; // ReaperDamageClass

namespace MightofUniverses.Common.GlobalItems
{
    public class CactusArmorGlobal : GlobalItem
    {
        public override bool InstancePerEntity => false;

        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.CactusHelmet:
                    item.defense = 1;
                    break;
                case ItemID.CactusBreastplate:
                    item.defense = 3;
                    break;
                case ItemID.CactusLeggings:
                    item.defense = 2;
                    break;
            }
        }

        public override void UpdateEquip(Item item, Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            switch (item.type)
            {
                case ItemID.CactusHelmet:
                    player.GetDamage(reaper) += 0.01f;
                    break;
                case ItemID.CactusBreastplate:
                    player.GetDamage(reaper) += 0.03f;
                    player.GetCritChance(reaper) += 2f;
                    break;
                case ItemID.CactusLeggings:
                    player.GetDamage(reaper) += 0.01f;
                    player.GetCritChance(reaper) += 1f;
                    player.moveSpeed += 0.05f;
                    break;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.CactusHelmet)
            {
                tooltips.Add(new TooltipLine(Mod, "CactusHelmet_Rework", "+1% reaper damage"));
            }
            else if (item.type == ItemID.CactusBreastplate)
            {
                tooltips.Add(new TooltipLine(Mod, "CactusBreastplate_Rework", "+3% reaper damage"));
                tooltips.Add(new TooltipLine(Mod, "CactusBreastplate_Rework2", "+2% reaper critical strike chance"));
            }
            else if (item.type == ItemID.CactusLeggings)
            {
                tooltips.Add(new TooltipLine(Mod, "CactusLeggings_Rework", "+1% reaper damage"));
                tooltips.Add(new TooltipLine(Mod, "CactusLeggings_Rework2", "+1% reaper critical strike chance"));
                tooltips.Add(new TooltipLine(Mod, "CactusLeggings_Rework3", "+5% movement speed"));
            }
        }
    }
}