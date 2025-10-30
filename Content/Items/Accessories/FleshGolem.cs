using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class FleshGolem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.endurance += 0.08f;
            player.lifeRegen += 3;
            player.statLifeMax2 += 25;
            player.GetModPlayer<FleshGolemPlayer>().hasFleshGolem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<NervousSystem>())
                .AddIngredient(ItemID.TissueSample, 10)
                .AddIngredient(ItemID.PanicNecklace)
                .AddIngredient(ItemID.Leather, 10)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
