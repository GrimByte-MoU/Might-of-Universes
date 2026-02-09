using Terraria;
using System;
using Terraria.ModLoader;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Common.GlobalItems
{
    public class SagittariusRarityGlobalItem : GlobalItem
    {
 public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
{
    if (item.rare != ModContent.GetInstance<SagittariusRarity>().Type || !(line.Mod == "Terraria" && line.Name == "ItemName"))
        return true;

    float pulseIntensity = 0.15f;
    float pulseScale = 1f + (float)Math.Sin(Main.GameUpdateCount * 0.08f) * pulseIntensity;
    line.BaseScale *= pulseScale;
    
    return true;
}


    }
}

