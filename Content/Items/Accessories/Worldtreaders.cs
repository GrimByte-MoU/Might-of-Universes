using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Accessories
{
    public class Worldtreaders : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(platinum: 1, gold: 50);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.accRunSpeed = 12f;
            player.moveSpeed += 0.20f;
            player.iceSkate = true;
            player.waterWalk = true;
            player.fireWalk = true;
            player.lavaMax += 900;
            player.lavaRose = true;
            player.noFallDmg = true;

            var modPlayer = player.GetModPlayer<WorldtreadersPlayer>();
            modPlayer.hasWorldtreaders = true;

            if (player.wingsLogic > 0)
            {
                player.wingTimeMax = (int)(player.wingTimeMax * 1.15f);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TerrasparkBoots, 1)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}