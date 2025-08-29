using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Ammo
{
    public class LightRound : ModItem
    {
        public override void SetStaticDefaults() 
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults() 
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<LightRoundProjectile>();
            Item.shootSpeed = 20f; // Very fast speed
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes() 
        {
            CreateRecipe(50)
                .AddIngredient<PureLight>(1)
                .AddIngredient<AetherLight>(1)
                .AddIngredient(ItemID.EmptyBullet, 50)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
