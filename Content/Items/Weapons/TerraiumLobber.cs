using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TerraiumLobber : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 300;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 32;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AncientSphere>();
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.useAmmo = AmmoID.Rocket;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-25f, -5f);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AncientSphere>(), damage, knockback, player.whoAmI);
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