using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;


namespace MightofUniverses.Content.Items.Ammo
{
    public class SollutumStake : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ModContent.RarityType<SollutumRarity>();
            Item.shoot = ModContent.ProjectileType<SollutumStakeProjectile>();
            Item.shootSpeed = 6f;
            Item.ammo = AmmoID.Stake;
        }

        public override void AddRecipes()
        {
            CreateRecipe(10)
                .AddIngredient(ItemID.Stake, 10)
                .AddIngredient(ModContent.ItemType<SollutumBar>(), 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
