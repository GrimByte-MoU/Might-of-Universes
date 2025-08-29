using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Accessories
{
    public class MushroomSample : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 45);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MushroomSamplePlayer>().hasMushroomSample = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GlowingMushroom, 13)
                .AddIngredient(ItemID.JungleSpores, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class MushroomSamplePlayer : ModPlayer
    {
        public bool hasMushroomSample;
        private int sporeTimer;

        public override void ResetEffects()
        {
            hasMushroomSample = false;
        }

        public override void PostUpdate()
        {
            if (!hasMushroomSample)
                return;

            sporeTimer++;
            if (sporeTimer >= 60)
            {
                sporeTimer = 0;
                if (Main.myPlayer == Player.whoAmI)
                {
                    Vector2 spawnPos = Player.Center + new Vector2(Main.rand.NextFloat(-15 * 16, 15 * 16), Main.rand.NextFloat(-15 * 16, 15 * 16));

                    Projectile.NewProjectile(
    Player.GetSource_Accessory(Player.inventory[Player.selectedItem]),
    spawnPos,
    Vector2.Zero,
    ModContent.ProjectileType<MushroomSpore>(),
    10,
    0f,
    Player.whoAmI
);
                }
            }
        }
    }
}
