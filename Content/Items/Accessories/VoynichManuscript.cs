using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class VoynichManuscript : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var book = player.GetModPlayer<BookEmpowermentPlayer>();
            book.hasBook = true;
            book.bonusRefreshTicks = 120;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Grimoire>()
                .AddIngredient(ItemID.AncientCloth, 7)
                .AddIngredient(ItemID.SolarTablet)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}