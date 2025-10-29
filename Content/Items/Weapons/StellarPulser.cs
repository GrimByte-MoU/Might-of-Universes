using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Weapons
{
    public class StellarPulser : ModItem
    {

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SuperStarCannon);
            Item.damage = 85;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 50);
            Item.useTime = 3;
            Item.useAnimation = 15;
            Item.reuseDelay = 20;
            Item.scale = 1.25f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float spreadDegrees = 4f;
            float spread = MathHelper.ToRadians(spreadDegrees);
            float rot1 = Main.rand.NextFloat(-spread, spread);
            Vector2 vel1 = velocity.RotatedBy(rot1);
            Projectile.NewProjectile(source, position, vel1, type, damage, knockback, player.whoAmI);
            float rot2 = Main.rand.NextFloat(-spread, spread);
            Vector2 vel2 = velocity.RotatedBy(rot2);
            Projectile.NewProjectile(source, position, vel2, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ItemID.SuperStarCannon);
            recipe.AddIngredient(ItemID.FragmentStardust, 10);
            recipe.AddIngredient(ItemID.FragmentVortex, 10);
            recipe.AddIngredient(ItemID.LunarBar, 8);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}