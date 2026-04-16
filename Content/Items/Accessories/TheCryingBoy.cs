using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class TheCryingBoy : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.BonusEmpowerDefense += 15;
            acc.BonusEmpowerEndurance += 0.12f;
            acc.BonusEmpowerLifeRegen += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AnguishedMan>()
                .AddIngredient(ItemID.LunarBar, 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}