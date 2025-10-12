using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Windlass : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Cutlass);
            Item.rare = ItemRarityID.Lime;
            Item.damage = 75;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.ArmorPenetration = 40;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cutlass, 1)
                .AddIngredient(ModContent.ItemType<GreedySpirit>(), 1) // TODO: set counts
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}