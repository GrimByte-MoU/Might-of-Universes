using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SkyAthame : ModItem
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
            player.GetModPlayer<SkyAthamePlayer>().hasSkyAthame = true;
            player.GetCritChance(DamageClass.Magic) += 20f;
            player.GetDamage(DamageClass.Magic) += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DestroyerEmblem)
                .AddIngredient(ItemID.SunplateBlock, 50)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    public class SkyAthamePlayer : ModPlayer
    {
        public bool hasSkyAthame = false;
        private int lastUseTime = 0;
        private const int UseCheckWindow = 120;

        public override void ResetEffects()
        {
            hasSkyAthame = false;
        }

        public override void PostItemCheck()
        {
            if (!hasSkyAthame) return;

            Item heldItem = Player.HeldItem;
            if (heldItem != null && !heldItem.IsAir && heldItem.DamageType == DamageClass.Magic && Player.itemAnimation > 0)
            {
                lastUseTime = 0;
            }
            else
            {
                lastUseTime++;
            }
        }

        public override void PostUpdateEquips()
        {
            if (!hasSkyAthame) return;

            if (lastUseTime < UseCheckWindow)
            {
                Player.moveSpeed += 0.35f;

                if (Main.rand.NextBool(3))
                {
                    Dust dust = Dust.NewDustDirect(
                        Player.position,
                        Player.width,
                        Player.height,
                        DustID.Cloud,
                        0f,
                        0f,
                        100,
                        default,
                        1.2f
                    );
                    dust.noGravity = true;
                    dust.velocity = -Player.velocity * 0.3f;
                }
            }
        }
    }
}