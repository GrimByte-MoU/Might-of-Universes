using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Rarities;
using MightofUniverses. Content.Items.Materials;
using MightofUniverses.Content.Items.Buffs;
using System.Collections.Generic;
using System.Linq;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TerraiumRifle : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 170;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 32;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TerraiumRifleShot>();
            Item.shootSpeed = 18f;
            Item.noMelee = true;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<TerraiumRifleShot>(), damage, knockback, player.whoAmI);
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