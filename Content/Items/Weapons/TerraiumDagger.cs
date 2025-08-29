using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class TerraiumDagger : ModItem
    {

        public override void SetDefaults()
        {
            Item.damage = 190;
            Item.DamageType = DamageClass.Melee;
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = false;
            Item.knockBack = 2.5f;
            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.shoot = ModContent.ProjectileType<TerraiumDaggerProjectile>();
            Item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int count = 5;
            float totalSpreadDegrees = 10f;
            float startAngle = -MathHelper.ToRadians(totalSpreadDegrees) / 2f;
            float step = MathHelper.ToRadians(totalSpreadDegrees) / (count - 1);

            for (int i = 0; i < count; i++)
            {
                float angle = startAngle + step * i;
                Vector2 perturbed = velocity.RotatedBy(angle);
                Vector2 spawnPos = position + perturbed.SafeNormalize(Vector2.Zero) * 10f;
                Projectile.NewProjectile(
                    source,
                    spawnPos,
                    perturbed,
                    type,
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
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
