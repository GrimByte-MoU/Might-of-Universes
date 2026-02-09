using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Audio;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Weapons
{
    public class EurekaBranch : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 85;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EurekaBolt>();
            Item.shootSpeed = 12f;
            Item.noMelee = true;
            Item.mana = 12;
            Item.maxStack = 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numBolts = Main.rand.Next(1, 4);
            float spreadAngle = MathHelper.ToRadians(15);

            for (int i = 0; i < numBolts; i++)
            {
                float speedMultiplier = Main.rand.NextFloat(0.6f, 1.2f);
                float angleOffset = spreadAngle * (i - (numBolts - 1) / 2f);
                Vector2 newVelocity = velocity.RotatedBy(angleOffset) * speedMultiplier;
                
                Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);
            }
            
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BrassBar>(), 15)
                .AddIngredient(ItemID.SoulofSight, 10)
                .AddIngredient(ItemID.SoulofLight, 25)
                .AddIngredient(ItemID.Cog, 25)
                .AddIngredient(ItemID.Wire, 50)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}

