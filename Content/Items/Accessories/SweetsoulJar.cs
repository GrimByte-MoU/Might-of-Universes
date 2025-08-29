using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SweetsoulJar : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<ReaperPlayer>();
            
            // Add soul energy every 60 ticks (1 second)
            if (player.whoAmI == Main.myPlayer && Main.GameUpdateCount % 60 == 0)
            {
                modPlayer.AddSoulEnergy(1f, player.Center);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GummyMembrane>(), 8)
                .AddIngredient(ItemID.Bottle, 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
