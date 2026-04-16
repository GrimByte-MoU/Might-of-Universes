using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class Grimoire : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var book = player.GetModPlayer<BookEmpowermentPlayer>();
            book.hasBook = true;
            book.bonusRefreshTicks = 90;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<CursedDiary>()
                .AddIngredient(ItemID.SpellTome)
                .AddIngredient(ItemID.SoulofNight, 7)
                .AddIngredient(ItemID.HallowedBar, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}