using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Accessories
{
    public class HematicLocket : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 90, 0);
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var p = player.GetModPlayer<ReaperAccessoryPlayer>();
            p.accHematicLocket = true;
            p.ApplyMaxSoulFromHP(0.15f);
        }

        public override void AddRecipes()
        {
            // Shadow Scale variant
            Recipe r1 = CreateRecipe();
            r1.AddIngredient(ItemID.LifeCrystal, 3);
            r1.AddIngredient(ItemID.ShadowScale, 5);
            r1.AddTile(TileID.Anvils);
            r1.Register();

            // Tissue Sample variant
            Recipe r2 = CreateRecipe();
            r2.AddIngredient(ItemID.LifeCrystal, 3);
            r2.AddIngredient(ItemID.TissueSample, 5);
            r2.AddTile(TileID.Anvils);
            r2.Register();
        }
    }
}