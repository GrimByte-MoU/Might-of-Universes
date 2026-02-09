using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;
using System;

namespace MightofUniverses.Content.Items.Weapons
{
    public class PrismaticFracture : ModItem
    {
        public override Vector2? HoldoutOffset() => new Vector2(10f, -6f);

        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item84;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PrismaticFractureProjectile1>();
            Item.shootSpeed = 12f;
            Item.mana = 14;
            Item.noMelee = true;
            Item.maxStack = 1;
        }

        public override void HoldItem(Player player)
        {
            var fracturePlayer = player.GetModPlayer<FracturePlayer>();

            if (player.itemAnimation == 0 && fracturePlayer.prismaticChargeTimer > 0)
            {
                fracturePlayer.prismaticChargeTimer = Math.Max(0, fracturePlayer.prismaticChargeTimer - 1);
            }
        }

        public override bool CanUseItem(Player player)
        {
            var fracturePlayer = player.GetModPlayer<FracturePlayer>();
            int minUseTime = 12;
            int baseUseTime = 40;
            int speedStep = fracturePlayer.prismaticChargeTimer / 30;

            Item.useTime = Math.Max(minUseTime, baseUseTime - speedStep * 2);
            Item.useAnimation = Item.useTime;

            return base.CanUseItem(player);
        }

        public override bool? UseItem(Player player)
        {
            var fracturePlayer = player.GetModPlayer<FracturePlayer>();
            fracturePlayer.prismaticChargeTimer += Item.useTime;
            return base.UseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int projectilesPerBurst = 5;
            float radius = 100f;

            for (int i = 0; i < projectilesPerBurst; i++)
            {
                float angle = MathHelper.TwoPi * i / projectilesPerBurst;
                Vector2 offset = Vector2.UnitX.RotatedBy(angle) * radius;
                Vector2 spawnPos = player.Center + offset;

                Vector2 direction = (Main.MouseWorld - spawnPos).SafeNormalize(Vector2.UnitY);
                Vector2 finalVelocity = direction * Item.shootSpeed;

                int randomProj = ModContent.ProjectileType<PrismaticFractureProjectile1>() + Main.rand.Next(10);

                Projectile.NewProjectile(source, spawnPos, finalVelocity, randomProj, damage, knockback, player.whoAmI);
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SkyFracture)
                .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}


