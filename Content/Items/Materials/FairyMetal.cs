using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Materials
{
    public class FairyMetal : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.LightRed;
        }
         public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PixieDust, 5)
                .AddIngredient(ModContent.ItemType<GoblinScrap>(), 3)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}