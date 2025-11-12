using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class MortalityBell : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 15);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.hasMortalityBell = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 3)
                .AddIngredient(ItemID.Bell, 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}