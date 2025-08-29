using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Materials
{
    public class PrismaticCatalyst : ModItem
    {
        public override void SetStaticDefaults() 
        {
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Cyan;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HallowedLight>(), 1)
                .AddIngredient(ModContent.ItemType<FestiveSpirit>(), 1)
                .AddIngredient(ModContent.ItemType<PureTerror>(), 1)
                .AddIngredient(ModContent.ItemType<EclipseLight>(), 1)
                .AddIngredient(ModContent.ItemType<GreedySpirit>(), 1)
                .AddIngredient(ModContent.ItemType<FrozenFragment>(), 1)
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}