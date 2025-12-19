using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.Players; // Adjust namespace if different

namespace MightofUniverses.Content.Items.Accessories
{
    public class LanternOfFadingMoths : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.value = Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            // Flat max soul increase
            reaper.maxSoulEnergy += 30f;

            // Time-based damage bonus
            if (!Main.dayTime)
            {
            player.GetDamage<ReaperDamageClass>() += 0.15f;
                // Night-only faint gray light
                Lighting.AddLight(player.Center, 0.15f, 0.15f, 0.15f);

                if (!hideVisual && Main.rand.NextBool(120))
                {
                    int d = Dust.NewDust(player.Center - new Vector2(8, 8), 16, 16, DustID.SilverCoin, 0f, -0.4f);
                    Main.dust[d].scale = 0.7f;
                    Main.dust[d].fadeIn = 0.9f;
                    Main.dust[d].velocity *= 0.2f;
                }
            }
            else
            {
                reaper.reaperDamageMultiplier += 0.03f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FireflyinaBottle, 1)
                .AddIngredient(ItemID.Chain, 1)
                .AddIngredient(ItemID.Lens, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}