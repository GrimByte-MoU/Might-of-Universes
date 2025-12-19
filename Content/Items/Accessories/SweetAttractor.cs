using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SweetAttractor : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 2;
            player.GetDamage(DamageClass.Summon) *= 0.9f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FallenStar, 5)
                 .AddIngredient(ModContent.ItemType<GoodnessFlower>(), 1)
              .AddIngredient(ModContent.ItemType<SweetgumBar>(), 5)
              .AddIngredient(ItemID.IceBlock, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
