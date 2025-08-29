using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Accessories
{
    public class Spookyfly : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 5);
            Item.DamageType = ModContent.GetInstance<PacifistDamageClass>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SpookyflyPlayer>().hasSpookyfly = true;
        }
        public override void AddRecipes()
{
    CreateRecipe()
        .AddIngredient(ItemID.Firefly, 1)
        .AddIngredient(ModContent.ItemType<PureTerror>(), 5)
        .AddTile(TileID.TinkerersWorkbench)
        .Register();
}

    }

    public class SpookyflyPlayer : ModPlayer
    {
        public bool hasSpookyfly;
        private int spookyflyCooldown = 0;

        public override void ResetEffects()
        {
            hasSpookyfly = false;
        }

        public override void PostUpdate()
        {
            if (!hasSpookyfly) return;

            if (spookyflyCooldown <= 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 spawnPos = Player.Center + Main.rand.NextVector2Circular(5 * 16, 5 * 16);
                    Vector2 velocity = Vector2.Zero;
                    Projectile.NewProjectile(
                        Player.GetSource_FromThis(),
                        spawnPos,
                        velocity,
                        ModContent.ProjectileType<SpookyflyMinion>(),
                        0,
                        0f,
                        Player.whoAmI
                    );
                }

                spookyflyCooldown = 60 * 5;
            }
            else spookyflyCooldown--;
        }

    }
    
}
