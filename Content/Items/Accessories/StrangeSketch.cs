using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class StrangeSketch : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.BonusEmpowerDefense += 6;
            acc.BonusEmpowerEndurance += 0.06f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<QuestionableInk>()
                .AddIngredient(ItemID.Leather, 10)
                .AddIngredient(ItemID.Silk, 5)
                .AddIngredient(ItemID.HellstoneBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}