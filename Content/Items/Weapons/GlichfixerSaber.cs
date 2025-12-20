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
    public class GlitchfixerSaber : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 150;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 8f;
            Item.maxStack = 1;
        }
            public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.rand.NextFloat() < 0.50f)
            {
                modifiers.FinalDamage *= 1.25f;
            }
            player.Heal((int)(modifiers.FinalDamage.Base * 0.05f));
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        
            if (Main.rand.NextFloat() < 0.2f)
            {
                Vector2 sphereVelocity = velocity * 0.5f;
                Projectile.NewProjectile(source, position, sphereVelocity, ModContent.ProjectileType<DeglitchingSphereMelee>(), damage, knockback, player.whoAmI);
            }
        
            return false; // or return true, depending on the desired behavior
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DebuggingSaber>())
                .AddIngredient(ItemID.LunarBar, 5)
                .AddIngredient(ModContent.ItemType<CleansedViralSaber>())
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}