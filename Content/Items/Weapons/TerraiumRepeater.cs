using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna. Framework;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Materials;
using System.Collections.Generic;
using MightofUniverses.Content.Items.Projectiles;


namespace MightofUniverses.Content. Items.Weapons
{
    public class TerraiumRepeater : ModItem
    {
        private int burstCounter = 0;

        public override void SetDefaults()
        {
            Item.damage = 165;
            Item. DamageType = DamageClass.Ranged;
            Item.width = 56;
            Item.height = 24;
            Item.useTime = 8;
            Item.useAnimation = 24;
            Item.reuseDelay = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(gold: 50);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AncientArrow>();
            Item.shootSpeed = 22f;
            Item.noMelee = true;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, Terraria. DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            burstCounter++;

            float spreadAngle = MathHelper.ToRadians(2);

            for (int i = 0; i < 2; i++)
            {
                float angleOffset = Main.rand.NextFloat(-spreadAngle, spreadAngle);
                Vector2 perturbedSpeed = velocity. RotatedBy(angleOffset);

                Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<AncientArrow>(), damage, knockback, player. whoAmI);
            }

            if (burstCounter >= 3)
            {
                burstCounter = 0;
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 12)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}