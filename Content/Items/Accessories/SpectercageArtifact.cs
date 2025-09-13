using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SpectercageArtifact : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.buyPrice(0, 4, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(ModContent.GetInstance<ReaperDamageClass>()) += 0.12f;
            player.GetModPlayer<ReaperAccessoryPlayer>().accSpectercageArtifact = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<ProtectionCharm>(), 1);
            r.AddIngredient(ModContent.ItemType<GreedySpirit>(), 5);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();
        }
    }
}