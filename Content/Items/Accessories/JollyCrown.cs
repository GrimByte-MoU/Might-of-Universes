using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class JollyCrown : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<JollyCrownPlayer>().hasJollyCrown = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoldCrown, 1)
                .AddIngredient(ModContent.ItemType<Materials.FestiveSpirit>(), 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.PlatinumCrown, 1)
                .AddIngredient(ModContent.ItemType<Materials.FestiveSpirit>(), 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
