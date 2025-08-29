using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Materials
{
    public class UnderworldMass : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 10);
            Item.rare = ItemRarityID.LightRed;
        }
         public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofFright, 1)
                .AddIngredient(ItemID.LivingFireBlock, 5)
                .AddIngredient(ModContent.ItemType<UnderworldSlag>(), 1)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}