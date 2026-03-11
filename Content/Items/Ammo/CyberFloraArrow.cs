using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Rarities;


namespace MightofUniverses.Content.Items.Ammo
{
    public class CyberFloraArrow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 38;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.shoot = ModContent.ProjectileType<CyberFloraArrowProjectile>();
            Item.shootSpeed = 5f;
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            CreateRecipe(50)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 1)
                .AddIngredient(ItemID.ChlorophyteBar, 1)
                .AddIngredient(ItemID.JungleSpores, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}