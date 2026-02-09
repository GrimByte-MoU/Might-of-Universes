using Terraria;
using System;
using Terraria.ModLoader;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Common.GlobalItems
{
    public class OmniverseRarityGlobalItem : GlobalItem
    {
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
{
    if (item.rare != ModContent.GetInstance<OmniverseRarity>().Type || !(line.Mod == "Terraria" && line.Name == "ItemName"))
        return true;

    float pulseIntensity = 0.2f;
    float pulseSpeed = 0.12f;
    float pulseScale = 1f + (float)Math.Sin(Main.GameUpdateCount * pulseSpeed) * pulseIntensity;
    line.BaseScale *= pulseScale;
    
    return true;
}

    }
}
