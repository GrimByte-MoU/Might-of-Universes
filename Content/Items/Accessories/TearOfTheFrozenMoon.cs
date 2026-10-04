using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Accessories
{
    public class TearOfTheFrozenMoon : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.value = Item.sellPrice(gold: 20);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TearOfTheFrozenMoonPlayer>().hasTearOfTheFrozenMoon = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FestiveSpirit>(), 5)
                .AddIngredient(ItemID.LunarBar, 7)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
