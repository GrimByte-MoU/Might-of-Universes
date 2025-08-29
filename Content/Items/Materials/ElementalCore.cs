using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Materials
{
    public class ElementalCore : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(gold: 7);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 1)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
