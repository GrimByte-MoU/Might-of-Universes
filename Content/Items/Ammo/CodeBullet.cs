using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Ammo
{
    public class CodeBullet : ModItem
    {
        public override void SetStaticDefaults() 
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults() 
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<CodeBulletProjectile>();
            Item.shootSpeed = 16f;
            Item.ammo = AmmoID.Bullet;
        }

        public override void AddRecipes() 
        {
            CreateRecipe(50)
                .AddIngredient<SyntheticumBar>(1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
