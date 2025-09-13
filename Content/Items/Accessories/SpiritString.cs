using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SpiritString : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 80, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var p = player.GetModPlayer<ReaperAccessoryPlayer>();
            p.accSpiritString = true;
            p.ApplyPassiveSoulToHPScalar(0.5f);
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ItemID.Silk, 10);
            r.AddIngredient(ItemID.FallenStar, 10);
            r.AddIngredient(ItemID.LifeCrystal, 3);
            r.AddTile(TileID.Loom);
            r.Register();
        }
    }
}