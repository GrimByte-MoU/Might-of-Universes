using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Weapons
{
    public class UnmeiTeki : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 175;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(0, 25, 0, 0);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<UnmeiPetal>();
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Bullet;
            Item.maxStack = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 5)
                .AddIngredient(ItemID.LifeCrystal, 5)
                .AddIngredient(ItemID.OrichalcumRepeater)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // short-range petals
            for (int i = 0; i < 2; i++)
            {
                Vector2 perturbed = velocity.RotatedByRandom(MathHelper.ToRadians(10)) * Main.rand.NextFloat(0.6f, 0.8f);
                Projectile.NewProjectile(source, position, perturbed,
                    ModContent.ProjectileType<UnmeiPetal>(), (int)(damage * 0.2f), knockback, player.whoAmI);
            }

            // long-range bloom
            if (Main.GameUpdateCount % 15 == 0)
            {
                Projectile.NewProjectile(source, position, velocity * 1.5f,
                    ModContent.ProjectileType<UnmeiBloom>(), (int)(damage * 2.0f), knockback, player.whoAmI);
            }
            return false;
        }
    }
}
