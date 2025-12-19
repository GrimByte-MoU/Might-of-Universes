using Terraria;
using Terraria. ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses. Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class LunaticBanner :  ModItem
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
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            player.GetDamage<ReaperDamageClass>() += 0.15f;
            player.GetCritChance<ReaperDamageClass>() += 10f;
            player.GetModPlayer<LunaticBannerPlayer>().hasLunaticBanner = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LunaticCloth>(), 5)
                .AddIngredient(ModContent. ItemType<ReaperEmblem>(), 1)
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

        public override void PostUpdateEquips()
        {
            if (hasLunaticBanner)
            {
                var reaperPlayer = Player.GetModPlayer<ReaperPlayer>();

                if (reaperPlayer.justConsumedSouls)
                {
                    int healAmount = (int)(Player.statLifeMax2 * 0.15f);

                    Player.statLife += healAmount;

                    if (Player.statLife > Player.statLifeMax2)
                    {
                        Player.statLife = Player.statLifeMax2;
                    }

                    if (healAmount > 0)
                    {
                        Player.HealEffect(healAmount);
                    }
                }
            }
        }
    }
}