using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Ammo
{
    public class ReinforcedStake : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(silver: 2);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<ReinforcedStakeProjectile>();
            Item.shootSpeed = 6f;
            Item.ammo = AmmoID.Stake;
        }

        public override void AddRecipes()
        {
            CreateRecipe(10)
                .AddIngredient(ItemID.Stake, 10)
                .AddRecipeGroup("IronBar", 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
