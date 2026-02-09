using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using MightofUniverses.Common.Players;
using System.Collections.Generic;

namespace MightofUniverses.Content.Items.Accessories
{
    public class UnstableCharm : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 10);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string keyName = ReaperPlayer.Ability2Key?.GetAssignedKeys().Count > 0 
                ? ReaperPlayer.Ability2Key.GetAssignedKeys()[0] 
                : "(unbound)";

            foreach (var line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                {
                    line.Text = line.Text.Replace("{keyName}", keyName);
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.hasUnstableCharm = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TeleportationPotion, 2)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.LunarBar, 3)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}