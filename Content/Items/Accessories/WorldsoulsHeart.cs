using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class WorldsoulsHeart : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(platinum: 1);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<ReaperPlayer>();
            
            if (modPlayer.justConsumedSouls)
            {
                player.AddBuff(ModContent.BuffType<WorldsoulsImmortality>(), 300);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TurtlesHeart>(), 1)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}