using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class AnguishedMan : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.BonusEmpowerDefense += 12;
            acc.BonusEmpowerEndurance += 0.10f;
            acc.BonusEmpowerLifeRegen += 6;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<HauntedPainting>()
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 5)
                .AddIngredient(ItemID.SpectreBar, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}