using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Emberfangs : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.shootSpeed = 1f;
            Item.shoot = ModContent.ProjectileType<EmberfangsBlade>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;
            Item.autoReuse = false;
        }

        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && 
                    Main.projectile[i].owner == player.whoAmI && 
                    Main.projectile[i].type == ModContent.ProjectileType<EmberfangsBlade>())
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int innerDamage = (int)(damage * 1.5f);
            Projectile.NewProjectile(
                source,
                player.Center,
                Vector2.Zero,
                ModContent.ProjectileType<EmberfangsBlade>(),
                innerDamage,
                knockback,
                player.whoAmI,
                ai0: 0f,
                ai1: 0f
            );
            int outerDamage = (int)(damage * 1f);
            Projectile.NewProjectile(
                source,
                player.Center,
                Vector2.Zero,
                ModContent.ProjectileType<EmberfangsBlade>(),
                outerDamage,
                knockback,
                player.whoAmI,
                ai0: 1f,
                ai1: 0f
            );

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ChainGuillotines, 1)
                .AddIngredient(ItemID.Sunfury, 1)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}