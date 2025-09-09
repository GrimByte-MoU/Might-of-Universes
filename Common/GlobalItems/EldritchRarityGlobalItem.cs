using Terraria;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using MightofUniverses.Content.Rarities;
using System.Linq;
using System.Collections.Generic;

namespace MightofUniverses.Common.GlobalItems
{
    public class EldritchRarityGlobalItem : GlobalItem
    {
       public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
{
    if (item.rare != ModContent.GetInstance<EldritchRarity>().Type)
        return;

    TooltipLine nameLine = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Name == "ItemName");
    if (nameLine != null)
    {
        Color pthaloBluePulse = Color.Lerp(new Color(0, 150, 150), new Color(0, 100, 100), (float)Math.Sin(Main.GameUpdateCount * 0.05f) * 0.5f + 0.5f);
        nameLine.OverrideColor = pthaloBluePulse;
    }
}

    }
}
