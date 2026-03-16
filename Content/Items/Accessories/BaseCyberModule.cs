using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class BaseCyberModule : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BaseCyberModulePlayer>().hasBaseCyberModule = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 7)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 7)
                .AddIngredient(ItemID.Nanites, 20)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}