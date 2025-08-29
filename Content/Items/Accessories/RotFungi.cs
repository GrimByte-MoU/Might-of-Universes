using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class RotFungi : ModItem
    {
   public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(silver: 2);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.5f;
            player.statDefense -= 5;
            player.statLifeMax2 -= 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShadowScale, 5)
                .AddIngredient(ItemID.GlowingMushroom, 10)
                .AddIngredient(ItemID.VileMushroom, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}