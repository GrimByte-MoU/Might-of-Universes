using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Ammo
{
    public class DeltaStormArrow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 38;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(silver: 6);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.shoot = ModContent.ProjectileType<DeltaStormArrowProjectile>();
            Item.shootSpeed = 3f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            CreateRecipe(50)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 1)
                .AddIngredient(ItemID.LunarBar, 1)
                .AddIngredient(ItemID.Cloud, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}