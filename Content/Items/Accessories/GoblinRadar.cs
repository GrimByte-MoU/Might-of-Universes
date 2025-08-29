using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class GoblinRadar : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.findTreasure = true;
            player.detectCreature = true;
            player.dangerSense = true;
            player.nightVision = true;
            player.sonarPotion = true;
            Lighting.AddLight(player.Center, 0.8f, 0.8f, 0.8f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoblinTool>())
                .AddIngredient(ItemID.IronBar, 10)
                .AddIngredient(ItemID.GoldBar)
                .AddIngredient(ItemID.Wire, 25)
                .AddIngredient(ItemID.NightOwlPotion)
                .AddIngredient(ItemID.TrapsightPotion)
                .AddIngredient(ItemID.HunterPotion)
                .AddIngredient(ItemID.SonarPotion)
                .AddIngredient(ItemID.MetalDetector)
                .AddIngredient(ItemID.LifeformAnalyzer)
                .AddIngredient(ItemID.Radar)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoblinTool>())
                .AddIngredient(ItemID.LeadBar, 10)
                .AddIngredient(ItemID.PlatinumBar)
                .AddIngredient(ItemID.Wire, 25)
                .AddIngredient(ItemID.NightOwlPotion)
                .AddIngredient(ItemID.TrapsightPotion)
                .AddIngredient(ItemID.HunterPotion)
                .AddIngredient(ItemID.SonarPotion)
                .AddIngredient(ItemID.MetalDetector)
                .AddIngredient(ItemID.LifeformAnalyzer)
                .AddIngredient(ItemID.Radar)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}