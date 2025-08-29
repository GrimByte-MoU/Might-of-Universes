using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.Audio;

namespace MightofUniverses.Content.Items.Weapons
{
    public class BloodedBlade : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 80;
            Item.height = 80;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(gold: 5);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 90;
            Item.knockBack = 4f;
            Item.crit = 6;
            Item.shoot = ModContent.ProjectileType<SanguineDroplet>();
            Item.shootSpeed = 16f;
        }

        public override void HoldItem(Player player)
{
    // Bright crimson lighting
    Lighting.AddLight(player.Center, 1f, 0.1f, 0.1f);

    // Crimson particles during use
    if (player.itemAnimation > 0)
    {
        Dust.NewDust(player.position, player.width, player.height, DustID.CrimsonTorch, 0f, 0f, 0, default, 1.5f);
        
        // Blood drips during swings
        if (Main.rand.NextBool(2)) // 50% chance each frame during swing
        {
            Dust.NewDust(player.position, player.width, player.height, DustID.Blood, 
                0f, 5f, 0, default, 1f); // Positive Y velocity makes it drip down
        }
    }
    
    // Occasional blood drips while holding
    if (Main.rand.NextBool(20)) // Periodic drips while holding
    {
        Vector2 dustPosition = player.Center;
        dustPosition.X += Main.rand.Next(-10, 11); // Random X position variation
        Dust.NewDust(dustPosition, 2, 2, DustID.Blood, 0f, 2f, 0, default, 1f);
    }
}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Main blood droplet
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);

            // Blood shards spread
            float startingAngle = -45f;
            for (int i = 0; i < 7; i++)
            {
                Vector2 shortVelocity = velocity * 0.3f;
                Vector2 rotatedVelocity = shortVelocity.RotatedBy(MathHelper.ToRadians(startingAngle + (i * 15f)));
                Projectile.NewProjectile(source, position, rotatedVelocity, 
                    ModContent.ProjectileType<BloodShard>(), damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void UseItemFrame(Player player)
        {
            SoundEngine.PlaySound(SoundID.NPCHit13, player.position);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 12)
                .AddIngredient(ItemID.Bottle, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}

