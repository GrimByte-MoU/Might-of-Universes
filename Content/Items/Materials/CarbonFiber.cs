using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Materials
{
    public class CarbonFiber : ModItem
    {
        public override void SetStaticDefaults() 
        {
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 30);
            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoblinScrap>(), 1)
                .AddIngredient(ModContent.ItemType<AcidVial>(), 1)
                .AddIngredient(ItemID.MythrilBar, 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}