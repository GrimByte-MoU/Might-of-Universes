using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class CursedDiary : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var book = player.GetModPlayer<BookEmpowermentPlayer>();
            book.hasBook = true;
            book.bonusRefreshTicks = 60;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<OldBook>()
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 7)
                .AddIngredient(ItemID.MythrilBar, 7)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient<OldBook>()
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 7)
                .AddIngredient(ItemID.OrichalcumBar, 7)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}