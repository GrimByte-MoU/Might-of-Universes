using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using Terraria.Localization;

namespace MightofUniverses.Content.Items.Accessories
{
    public class GoblinCore : ModItem
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
            player.moveSpeed += 0.15f;
            player.accFlipper = true;
            player.accDepthMeter = 1;
            player.lavaImmune = true;
            player.waterWalk = true;
            player.noFallDmg = true;
            player.archery = true;
            player.accWatch = 1;
            Lighting.AddLight(player.Center, 0.8f, 0.8f, 0.8f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoblinTool>())
                .AddIngredient(ItemID.FeatherfallPotion)
                .AddIngredient(ItemID.FlipperPotion)
                .AddIngredient(ItemID.SwiftnessPotion)
                .AddIngredient(ItemID.WaterWalkingPotion)
                .AddIngredient(ItemID.ObsidianSkinPotion)
                .AddIngredient(ItemID.Stopwatch)
                .AddIngredient(ItemID.DepthMeter)
                .AddIngredient(ItemID.Cloud, 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}