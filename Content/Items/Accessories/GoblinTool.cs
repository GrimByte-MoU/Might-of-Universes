using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class GoblinTool : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.findTreasure = true;
            player.pickSpeed -= 0.25f;
            Lighting.AddLight(player.Center, 0.8f, 0.8f, 0.8f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoblinScrap>(), 5)
                .AddIngredient(ModContent.ItemType<GoblinLeather>(), 5)
                .AddIngredient(ItemID.GoldBar, 3)
                .AddIngredient(ItemID.SpelunkerPotion)
                .AddIngredient(ItemID.ShinePotion)
                .AddIngredient(ItemID.MiningPotion)
                .AddIngredient(ItemID.Glass, 3)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoblinScrap>(), 5)
                .AddIngredient(ModContent.ItemType<GoblinLeather>(), 5)
                .AddIngredient(ItemID.PlatinumBar, 3)
                .AddIngredient(ItemID.SpelunkerPotion)
                .AddIngredient(ItemID.ShinePotion)
                .AddIngredient(ItemID.MiningPotion)
                .AddIngredient(ItemID.Glass, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
