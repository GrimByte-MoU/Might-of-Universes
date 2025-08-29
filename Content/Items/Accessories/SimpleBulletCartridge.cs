using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SimpleBulletCartridge : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BulletPiercingPlayer>().bulletPiercing += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MusketBall, 50)
                .AddIngredient(ItemID.IronBar, 5) 
                .AddTile(TileID.Anvils)
                .Register();

                CreateRecipe()
                .AddIngredient(ItemID.MusketBall, 50)
                .AddIngredient(ItemID.LeadBar, 5) 
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
