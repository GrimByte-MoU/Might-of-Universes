using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Ammo
{
    public class MoltenCoreArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 38;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(silver: 7);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.shoot = ModContent.ProjectileType<MoltenCoreArrowProjectile>();
            Item.shootSpeed = 5.5f;
            Item.ammo = AmmoID.Arrow;
            Item.crit = 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe(50)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 1)
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 1)
                .AddIngredient(ItemID.LivingFireBlock, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}