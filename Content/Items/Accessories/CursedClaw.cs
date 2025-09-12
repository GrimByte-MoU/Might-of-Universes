using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class CursedClaw : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 6, silver: 50);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.maxSoulEnergy += 75f;
            reaper.reaperDamageMultiplier += 0.15f;
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.SoulCostMultiplier *= 1.10f;
            player.statLifeMax2 = (int)(player.statLifeMax2 * 1.15f);
            player.endurance -= 0.30f;
            if (player.endurance < -0.90f)
                player.endurance = -0.90f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MonkeysPaw>(), 1)
                .AddIngredient(ModContent.ItemType<DevilsBlood>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}