using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Ammo
{
    public class NegativeColdArrow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 38;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(silver: 6);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.shoot = ModContent.ProjectileType<NegativeColdArrowProjectile>();
            Item.shootSpeed = 5f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            CreateRecipe(50)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 1)
                .AddIngredient(ModContent.ItemType<FrozenFragment>(), 1)
                .AddIngredient(ItemID.IceBlock, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}