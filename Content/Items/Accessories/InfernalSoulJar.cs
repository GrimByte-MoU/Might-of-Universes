using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class InfernalSoulJar : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<ReaperPlayer>();
            
            // Add soul energy every 60 ticks (1 second)
            if (player.whoAmI == Main.myPlayer && Main.GameUpdateCount % 20 == 0)
            {
                modPlayer.AddSoulEnergy(1f, player.Center);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SweetsoulJar>(), 1)
                .AddIngredient(ModContent.ItemType<DemonicEssence>(), 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}