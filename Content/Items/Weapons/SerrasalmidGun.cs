using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class SerrasalmidGun : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.PiranhaGun);
            Item.damage = 65;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(gold: 10);

            Item.shoot = ModContent.ProjectileType<SerrasalmidPiranha>();
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
        }

        public override bool CanUseItem(Player player)
        {
            int projType = ModContent.ProjectileType<SerrasalmidPiranha>();
            return player.ownedProjectileCounts[projType] == 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.whoAmI != Main.myPlayer)
                return false;

            int projType = ModContent.ProjectileType<SerrasalmidPiranha>();
            Vector2 dir = velocity.LengthSquared() > 0.01f
                ? Vector2.Normalize(velocity)
                : Vector2.Normalize(Main.MouseWorld - player.MountedCenter);

            float speed = Item.shootSpeed > 0 ? Item.shootSpeed : 12f;

            for (int i = 0; i < 5; i++)
            {
                float t = i / 4f;
                Vector2 v = dir.RotatedBy(MathHelper.Lerp(-0.2f, 0.2f, t)) * speed;
                Projectile.NewProjectile(source, position, v, projType, damage, knockback, player.whoAmI);
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PiranhaGun, 1)
                .AddIngredient(ModContent.ItemType<AncientBone>(), 10)
                .AddIngredient(ModContent.ItemType<PrehistoricAmber>(), 8)
                .AddIngredient(ModContent.ItemType<TarChunk>(), 6)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}