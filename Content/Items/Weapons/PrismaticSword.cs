using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;
using Terraria.DataStructures;
using System;
using Terraria.Audio;


namespace MightofUniverses.Content.Items.Weapons
{
    public class PrismaticSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 100;
            Item.height = 100;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(gold: 25);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 120;
            Item.knockBack = 8f;
            Item.crit = 10;
            Item.shootSpeed = 20f;
            Item.shoot = ProjectileID.PurificationPowder;
        }

        public override void HoldItem(Player player)
        {
            // Bright rainbow lighting
            float r = Main.DiscoR / 255f;
            float g = Main.DiscoG / 255f;
            float b = Main.DiscoB / 255f;
            Lighting.AddLight(player.Center, r * 1.5f, g * 1.5f, b * 1.5f);

           // Toned down rainbow particles
        if (Main.rand.NextBool(3))
{
    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.RainbowTorch, 0f, 0f, 100, default, 0.8f);
    dust.noGravity = true;
    dust.fadeIn = 0.2f;
    dust.active = true;
}


        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<PrismaticRend>(), 180);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int[] projectileTypes = new int[]
            {
                ModContent.ProjectileType<RedPrismaticProjectile>(),
                ModContent.ProjectileType<OrangePrismaticProjectile>(),
                ModContent.ProjectileType<YellowPrismaticProjectile>(),
                ModContent.ProjectileType<GreenPrismaticProjectile>(),
                ModContent.ProjectileType<BluePrismaticProjectile>(),
                ModContent.ProjectileType<PurplePrismaticProjectile>()
            };

            int selectedProjectile = Main.rand.Next(projectileTypes);

            if (selectedProjectile == ModContent.ProjectileType<GreenPrismaticProjectile>())
            {
                Projectile.NewProjectile(source, position, velocity, selectedProjectile, damage, knockback, player.whoAmI);
                Vector2 rotatedVelocity = velocity.RotatedBy(-10f * (Math.PI / 180f));
                Projectile.NewProjectile(source, position, rotatedVelocity, selectedProjectile, damage, knockback, player.whoAmI);
                rotatedVelocity = velocity.RotatedBy(10f * (Math.PI / 180f));
                Projectile.NewProjectile(source, position, rotatedVelocity, selectedProjectile, damage, knockback, player.whoAmI);
                return false;
            }
            
            if (selectedProjectile == ModContent.ProjectileType<BluePrismaticProjectile>())
            {
                Vector2 rotatedVelocity = velocity.RotatedBy(-5f * (Math.PI / 180f));
                Projectile.NewProjectile(source, position, rotatedVelocity, selectedProjectile, damage, knockback, player.whoAmI);
                rotatedVelocity = velocity.RotatedBy(5f * (Math.PI / 180f));
                Projectile.NewProjectile(source, position, rotatedVelocity, selectedProjectile, damage, knockback, player.whoAmI);
                return false;
            }

            Projectile.NewProjectile(source, position, velocity, selectedProjectile, damage, knockback, player.whoAmI);
            return false;
        }

        public override void UseItemFrame(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item1, player.position);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TerraBlade)
                .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
