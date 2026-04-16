using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class BookOfSoyga : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var book = player.GetModPlayer<BookEmpowermentPlayer>();
            book.hasBook = true;
            book.bonusRefreshTicks = 120;
            book.hasRefundChance = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<VoynichManuscript>()
                .AddIngredient(ItemID.LunarBar, 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}