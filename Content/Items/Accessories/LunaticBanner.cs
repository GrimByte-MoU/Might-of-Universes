using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class LunaticBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Get the ReaperPlayer instance for this player
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();

            // Increase Reaper damage by 15%
            reaperPlayer.reaperDamageMultiplier += 0.15f;

            // Increase Reaper critical strike chance by 10%
            reaperPlayer.reaperCritChance += 10;

            // Enable soul energy healing effect
            player.GetModPlayer<LunaticBannerPlayer>().hasLunaticBanner = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LunaticCloth>(), 5) // Requires 5 Lunatic Cloth
                .AddIngredient(ModContent.ItemType<ReaperEmblem>(), 1) // Requires 1 Reaper Emblem
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class LunaticBannerPlayer : ModPlayer
    {
        public bool hasLunaticBanner = false;

        public override void ResetEffects()
        {
            hasLunaticBanner = false;
        }

        // Hook into the soul energy consumption event
        public override void PostUpdateEquips()
        {
            if (hasLunaticBanner)
            {
                // Get the ReaperPlayer instance to access soul energy information
                var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();

                // Check if soul energy was consumed this frame
                if (reaperPlayer.justConsumedSouls)
                {
                    // Calculate healing amount (15% of max health)
                    int healAmount = (int)(Player.statLifeMax2 * 0.15f);

                    // Heal the player
                    Player.statLife += healAmount;

                    // Cap health at max health
                    if (Player.statLife > Player.statLifeMax2)
                    {
                        Player.statLife = Player.statLifeMax2;
                    }

                    // Show healing text
                    if (healAmount > 0)
                    {
                        Player.HealEffect(healAmount);
                    }
                }
            }
        }
    }
}
