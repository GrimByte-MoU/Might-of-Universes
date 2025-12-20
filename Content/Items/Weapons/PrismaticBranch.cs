using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class PrismaticBranch : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 54;
            Item.DamageType = DamageClass.Magic;
            Item.width = 44;
            Item.height = 44;
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item33;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<PrismaticNeedleRed>();
            Item.shootSpeed = 16f;
            Item.mana = 5;
            Item.maxStack = 1;
        }

        private int specialSprayCooldown = 0;

        public override void UpdateInventory(Player player)
        {
            if (specialSprayCooldown > 0)
                specialSprayCooldown--;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Main needle spray — like Razorpine
            int mainType = GetRandomNeedle();
            Vector2 spreadVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(9));
            Projectile.NewProjectile(source, position, spreadVelocity, mainType, damage, knockback, player.whoAmI);

            // Special burst every 60 ticks
            if (specialSprayCooldown <= 0)
            {
                specialSprayCooldown = 60;

                for (int i = 0; i < 5; i++)
                {
                    int specialType = GetRandomNeedle();
                    Vector2 offsetVel = velocity.RotatedBy(MathHelper.ToRadians(-20 + i * 10)) * 0.5f;
                    int p = Projectile.NewProjectile(source, position, offsetVel, specialType, damage, knockback, player.whoAmI);
                    Main.projectile[p].ai[0] = 1f; // flag it for acceleration
                }
            }

            return false; // prevent vanilla projectile
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Razorpine)
                .AddIngredient(ModContent.ItemType<SprinkleShaker>())
                .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        private int GetRandomNeedle()
        {
            int[] types = new int[]
            {
                ModContent.ProjectileType<PrismaticNeedleRed>(),
                ModContent.ProjectileType<PrismaticNeedleOrange>(),
                ModContent.ProjectileType<PrismaticNeedleYellow>(),
                ModContent.ProjectileType<PrismaticNeedleGreen>(),
                ModContent.ProjectileType<PrismaticNeedleBlue>(),
                ModContent.ProjectileType<PrismaticNeedlePurple>()
            };

            return Main.rand.Next(types);
        }
    }
}

