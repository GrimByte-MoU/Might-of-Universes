using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Buffs;
using System.Collections.Generic;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items. Weapons
{
    public class ElementalFan : ModItem
    {
        private int shotCounter = 0;

        public override void SetDefaults()
        {
            Item.damage = 85;
            Item.DamageType = DamageClass.Magic;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.useStyle = ItemUseStyleID. Shoot;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ElementalFanNeedle>();
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.mana = 4;
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            shotCounter++;

            if (shotCounter >= 8)
            {
                shotCounter = 0;
                
                for (int i = 0; i < 10; i++)
                {
                    float spreadAngle = MathHelper.ToRadians(5);
                    float angleOffset = MathHelper. Lerp(-spreadAngle, spreadAngle, i / 10f);
                    Vector2 perturbedSpeed = velocity. RotatedBy(angleOffset);
                    
                    int proj = Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
                    if (proj >= 0 && proj < Main.maxProjectiles)
                    {
                        Main.projectile[proj].ai[0] = Main.rand.Next(6);
                    }
                }
            }
            else
            {
                int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (proj >= 0 && proj < Main.maxProjectiles)
                {
                    Main.projectile[proj].ai[0] = Main.rand.Next(6);
                }
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PrismaticBranch>())
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}