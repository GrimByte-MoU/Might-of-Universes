using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Weapons
{
    public class AetherialSerpent : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.CrystalSerpent);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<AetherSerpentMain>();
            Item.damage = 60;
            Item.useTime = 20;
            Item.useAnimation = 20;
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AetherSerpentMain>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrystalSerpent, 1)
                .AddIngredient(ModContent.ItemType<HallowedLight>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}