using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class TatteredCharm : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 60, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ReaperAccessoryPlayer>().accTatteredCharm = true;
        }

        public override void AddRecipes()
        {
            // Rotten Chunk variant
            Recipe r1 = CreateRecipe();
            r1.AddIngredient(ItemID.FallenStar, 3);
            r1.AddIngredient(ItemID.RottenChunk, 5);
            r1.AddTile(TileID.WorkBenches);
            r1.Register();

            // Vertebrae variant
            Recipe r2 = CreateRecipe();
            r2.AddIngredient(ItemID.FallenStar, 3);
            r2.AddIngredient(ItemID.Vertebrae, 5);
            r2.AddTile(TileID.WorkBenches);
            r2.Register();
        }
    }
}