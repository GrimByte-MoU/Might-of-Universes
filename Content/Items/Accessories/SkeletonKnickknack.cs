using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SkeletonKnickknack : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 1, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var p = player.GetModPlayer<ReaperAccessoryPlayer>();
            p.accSkeletonKnickknack = true;
            p.flatMaxSoulsBonus += 30;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<SoulSiphoningArtifact>(), 1);
            r.AddIngredient(ItemID.SoulofLight, 5);
            r.AddIngredient(ItemID.Bone, 15);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();
        }
    }
}