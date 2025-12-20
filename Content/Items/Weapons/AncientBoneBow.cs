using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class AncientBoneBow : ModItem
    {

        private const float TarSpreadDegrees = 5f;

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 60;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 10;
            Item.useAnimation = 20;
            Item.reuseDelay = 20;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item5;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 65;
            Item.knockBack = 4f;
            Item.crit += 6;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Arrow;

            Item.noMelee = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 10);
            Item.maxStack = 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int projType = type == ProjectileID.WoodenArrowFriendly
                ? ModContent.ProjectileType<AncientBoneArrow>()
                : type;
            Projectile.NewProjectile(source, position, velocity, projType, damage, knockback, player.whoAmI);
            Vector2 baseDir = velocity.SafeNormalize(Vector2.UnitX);
            float speed = velocity.Length();
            if (speed < 0.01f)
                speed = Item.shootSpeed;

            float half = TarSpreadDegrees * 0.5f;
            for (int i = 0; i < 2; i++)
            {
                float angleDeg = (i == 0) ? -half : +half;
                Vector2 tarVel = baseDir.RotatedBy(MathHelper.ToRadians(angleDeg)) * speed;

                Projectile.NewProjectile(
                    source,
                    position,
                    tarVel,
                    ModContent.ProjectileType<TarSplatter>(),
                    damage, 
                    knockback,
                    player.whoAmI
                );
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Marrow, 1)
                .AddIngredient(ModContent.ItemType<PrehistoricAmber>(), 5)
                .AddIngredient(ModContent.ItemType<AncientBone>(), 10)
                .AddIngredient(ModContent.ItemType<TarChunk>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}