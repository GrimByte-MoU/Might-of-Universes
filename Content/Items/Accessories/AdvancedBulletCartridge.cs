using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class AdvancedBulletCartridge : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BulletPiercingPlayer>().bulletPiercing += 3;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SimpleBulletCartridge>())
                .AddIngredient(ModContent.ItemType<CarbonFiber>(), 5)
                .AddIngredient(ModContent.ItemType<Kevlar>(), 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
