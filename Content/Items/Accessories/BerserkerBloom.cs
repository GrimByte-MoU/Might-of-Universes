using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class BerserkerBloom : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BerserkerBloomPlayer>().hasBerserkerBloom = true;
            player.statLifeMax2 += 200;
            player.statDefense -= 15;
            player.endurance -= 0.15f;
            player.aggro += 1200;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GlommerPetItem)
                .AddIngredient(ItemID.JungleRose)
                .AddIngredient(ItemID.JungleSpores, 10)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class BerserkerBloomPlayer : ModPlayer
    {
        public bool hasBerserkerBloom = false;

        public override void ResetEffects()
        {
            hasBerserkerBloom = false;
        }

        public override void PostUpdateEquips()
        {
            if (!hasBerserkerBloom) return;

            float healthPercent = (float)Player.statLife / Player.statLifeMax2;

            if (healthPercent <= 0.50f)
            {
                Player.GetDamage(ModContent.GetInstance<ReaperDamageClass>()) += 0.25f;
            }

            if (healthPercent <= 0.25f)
            {
                Player.GetCritChance(ModContent.GetInstance<ReaperDamageClass>()) += 0.25f;
            }
        }

        public override void PostUpdate()
        {
            if (!hasBerserkerBloom) return;

            float healthPercent = (float)Player.statLife / Player.statLifeMax2;

            if (healthPercent <= 0.50f && Main.rand.NextBool(10))
            {
                Dust dust = Dust.NewDustDirect(
                    Player.position,
                    Player.width,
                    Player.height,
                    DustID.JunglePlants,
                    0f,
                    0f,
                    100,
                    default,
                    1.5f
                );
                dust.noGravity = true;
                dust.velocity *= 0.5f;
            }

            if (healthPercent <= 0.25f && Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustDirect(
                    Player.position,
                    Player.width,
                    Player.height,
                    DustID.Blood,
                    0f,
                    0f,
                    100,
                    default,
                    2f
                );
                dust.noGravity = true;
                dust.velocity = Player.velocity * 0.3f;
            }
        }
    }
}