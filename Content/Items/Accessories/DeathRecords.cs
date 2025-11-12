using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class DeathRecords : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 10);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            int deathMarks = reaperPlayer.deathMarks;
            player.GetDamage<ReaperDamageClass>() += 0.07f * deathMarks;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 15)
                .AddIngredient(ItemID.Book, 3)
                .AddIngredient(ItemID.LunarBar, 3)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}