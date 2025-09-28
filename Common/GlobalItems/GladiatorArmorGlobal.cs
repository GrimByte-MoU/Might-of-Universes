using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;

namespace MightofUniverses.Common.GlobalItems
{
    public class GladiatorArmorGlobal : GlobalItem
    {
        public override bool InstancePerEntity => false;

        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.GladiatorHelmet:
                    item.defense = 6;
                    break;
                case ItemID.GladiatorBreastplate:
                    item.defense = 8;
                    break;
                case ItemID.GladiatorLeggings:
                    item.defense = 6;
                    break;
            }
        }

        public override void UpdateEquip(Item item, Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            switch (item.type)
            {
                case ItemID.GladiatorHelmet:
                    player.GetDamage(reaper) += 0.03f;
                    player.GetCritChance(reaper) += 1f;
                    player.statLifeMax2 += 5;
                    break;
                case ItemID.GladiatorBreastplate:
                    player.GetDamage(reaper) += 0.04f;
                    player.GetCritChance(reaper) += 3f;
                    player.statLifeMax2 += 5;
                    break;
                case ItemID.GladiatorLeggings:
                    player.GetDamage(reaper) += 0.03f;
                    player.GetCritChance(reaper) += 2f;
                    player.moveSpeed += 0.08f;
                    player.statLifeMax2 += 5;
                    break;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.GladiatorHelmet)
            {
                tooltips.Add(new TooltipLine(Mod, "GladiatorHelmet_Rework", "+3% reaper damage"));
                tooltips.Add(new TooltipLine(Mod, "GladiatorHelmet_Rework2", "+1% reaper critical strike chance"));
                tooltips.Add(new TooltipLine(Mod, "GladiatorHelmet_Rework3", "+5 max life"));
            }
            else if (item.type == ItemID.GladiatorBreastplate)
            {
                tooltips.Add(new TooltipLine(Mod, "GladiatorBreastplate_Rework", "+4% reaper damage"));
                tooltips.Add(new TooltipLine(Mod, "GladiatorBreastplate_Rework2", "+3% reaper critical strike chance"));
                tooltips.Add(new TooltipLine(Mod, "GladiatorBreastplate_Rework3", "+5 max life"));
            }
            else if (item.type == ItemID.GladiatorLeggings)
            {
                tooltips.Add(new TooltipLine(Mod, "GladiatorLeggings_Rework", "+3% reaper damage"));
                tooltips.Add(new TooltipLine(Mod, "GladiatorLeggings_Rework2", "+2% reaper critical strike chance"));
                tooltips.Add(new TooltipLine(Mod, "GladiatorLeggings_Rework3", "+5 max life"));
                tooltips.Add(new TooltipLine(Mod, "GladiatorLeggings_Rework4", "+8% movement speed"));
            }
        }
    }
}