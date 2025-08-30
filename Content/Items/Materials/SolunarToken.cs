using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Materials
{
    public class SolunarToken : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Green;
        }

         public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SunSigil>(), 1)
                .AddIngredient(ModContent.ItemType<MoonSigil>(), 1)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }
}