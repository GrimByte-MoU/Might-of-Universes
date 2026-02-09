using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;
using System;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.ModLoader.IO;


namespace MightofUniverses.Content.Items.Weapons
{
    public class DebuggingSaber : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 70;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 8f;
            Item.maxStack = 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        
            if (Main.rand.NextFloat() < 0.15f)
            {
                Vector2 sphereVelocity = velocity * 0.5f;
                Projectile.NewProjectile(source, position, sphereVelocity, ModContent.ProjectileType<DebuggingSphereMelee>(), damage, knockback, player.whoAmI);
            }
        
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralSaber>())
                .AddIngredient(ItemID.ChlorophyteBar, 5)
                .AddIngredient(ModContent.ItemType<GlitchyChunk>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
