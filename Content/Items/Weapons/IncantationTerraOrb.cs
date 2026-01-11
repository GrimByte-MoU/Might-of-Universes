using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Materials;
using System.Collections.Generic;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class IncantationTerraOrb : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 140;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID. Shoot;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item84;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<TerraOrbProjectile>();
            Item.shootSpeed = 1f;
            Item.noMelee = true;
            Item.mana = 40;
        }

        public override bool Shoot(Player player, Terraria. DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player. whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 12)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}