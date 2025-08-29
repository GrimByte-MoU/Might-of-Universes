using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class UmbraOffering : ModItem
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
            player.GetModPlayer<UmbraOfferingPlayer>().hasUmbraOffering = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoldDust, 1)
                .AddIngredient(ModContent.ItemType<Materials.EclipseLight>(), 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
