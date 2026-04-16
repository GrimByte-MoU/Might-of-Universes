using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class UnusualLookingGlass : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var mirror = player.GetModPlayer<MirrorReflectPlayer>();
            mirror.hasMirror = true;
            mirror.reflectPercent = 0.75f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<FoggyGlass>()
                .AddIngredient(ItemID.MagicMirror)
                .AddIngredient(ItemID.ShadowScale, 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient<FoggyGlass>()
                .AddIngredient(ItemID.MagicMirror)
                .AddIngredient(ItemID.TissueSample, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}