using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;
using System;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Weapons
{
    public class HeadModsGlitchfixer : ModItem
    {
        private bool rapidFire = false;
        private int rapidFireTimer = 0;
        private const int RAPID_FIRE_DURATION = 40; // 1/2 second
        private const int NORMAL_USE_TIME = 15;
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 20;
            Item.damage = 50;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = NORMAL_USE_TIME;
            Item.useAnimation = NORMAL_USE_TIME;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Bullet;
            Item.maxStack = 1;
        }

         public override void UpdateInventory(Player player)
        {
            if (rapidFire)
            {
                rapidFireTimer++;
                if (rapidFireTimer >= RAPID_FIRE_DURATION)
                {
                    Item.useTime = NORMAL_USE_TIME;
                    Item.useAnimation = NORMAL_USE_TIME;
                    rapidFire = false;
                    rapidFireTimer = 0;
                }
            }
        }

       public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
       {
           Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
       
           if (Main.rand.NextFloat() < 0.1f)
           {
               Vector2 sphereVelocity = velocity * 0.5f;
               Projectile.NewProjectile(source, position, sphereVelocity, ModContent.ProjectileType<DeglitchingSphereRanged>(), damage, knockback, player.whoAmI);
           }
       
           int numShots = 3;
           float rand = Main.rand.NextFloat();
       
           if (rand < 0.30f) numShots = 4;
           else if (rand < 0.20f) numShots = 5;
           else if (rand < 0.10f) numShots = 6;
       
           for (int i = 0; i < numShots; i++)
           {
               Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(2));
               Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);
           }
       
           if (Main.rand.NextFloat() < 0.05f && !rapidFire)
           {
               Item.useTime = 1;
               Item.useAnimation = 1;
               rapidFire = true;
               rapidFireTimer = 0;
           }
       
           return false;
       }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<VRDebugger>())
                .AddIngredient(ItemID.LunarBar, 5)
                .AddIngredient(ModContent.ItemType<VRGlitchBlaster>())
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}