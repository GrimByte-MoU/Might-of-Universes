using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Accessories
{
    public class MyrtlesPlantationMirror : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var mirror = player.GetModPlayer<MirrorReflectPlayer>();
            mirror.hasMirror = true;
            mirror.reflectPercent = 2.00f;
            mirror.bonusIFrames = 90;
            mirror.grantSpeedBoost = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<MarysMirror>()
                .AddIngredient(ItemID.LunarBar, 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}