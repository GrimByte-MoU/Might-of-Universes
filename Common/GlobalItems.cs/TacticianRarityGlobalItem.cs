using Terraria;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using MightofUniverses.Content.Rarities;
using System.Linq;
using System.Collections.Generic;

namespace MightofUniverses.Common.GlobalItems
{
    public class TacticianRarityGlobalItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
{
    if (item.rare != ModContent.GetInstance<TacticianRarity>().Type)
        return;

    TooltipLine nameLine = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Name == "ItemName");
    if (nameLine != null)
    {
        Color goldPulse = Color.Lerp(new Color(255, 215, 0), new Color(218, 165, 32), (float)Math.Sin(Main.GameUpdateCount * 0.05f) * 0.5f + 0.5f);
        nameLine.OverrideColor = goldPulse;
    }
}

    }
}
