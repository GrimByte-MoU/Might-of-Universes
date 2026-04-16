using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class OldBook : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var book = player.GetModPlayer<BookEmpowermentPlayer>();
            book.hasBook = true;
            book.bonusRefreshTicks = 30;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Leather, 5)
                .AddIngredient(ItemID.Book)
                .AddCondition(Condition.InGraveyard)
                .Register();
        }
    }
}