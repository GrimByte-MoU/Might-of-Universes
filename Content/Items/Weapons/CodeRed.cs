using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;
using System;
using Terraria.DataStructures;
using MightofUniverses.Content.Rarities;
using Terraria.Audio;

namespace MightofUniverses.Content.Items.Weapons
{
    public class CodeRed : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 20;
            Item.damage = 400;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(platinum: 20);
            Item.rare = ModContent.RarityType<StarsteelRarity>();
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 25f;
            Item.useAmmo = AmmoID.Bullet;
            Item.scale = 0.5f;
            Item.maxStack = 1;
        }

       public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
       {
           Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
       
           if (Main.rand.NextFloat() < 0.05f)
           {
               Vector2 sphereVelocity = velocity * 0.35f;
               Projectile.NewProjectile(source, position, sphereVelocity, ModContent.ProjectileType<CodeRedSphere>(), damage, knockback, player.whoAmI);
           }
       
           int numShots = 5;
           float rand = Main.rand.NextFloat();
       
           if (rand < 0.30f) numShots = 6;
           else if (rand < 0.20f) numShots = 8;
           else if (rand < 0.10f) numShots = 10;
       
           for (int i = 0; i < numShots; i++)
           {
               Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(2));
               Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);
           }
           return false;
       }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HeadModsGlitchfixer>())
                .AddIngredient(ModContent.ItemType<AlienMetal>(), 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}