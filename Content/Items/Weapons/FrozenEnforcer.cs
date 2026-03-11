using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class FrozenEnforcer : ModItem
    {
        private int shotsFired = 0;
        private const int MaxShots = 6;

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 20;
            Item.damage = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 25f;
            Item.useAmmo = AmmoID.Bullet;
            Item.maxStack = 1;
            Item.scale = 1f;
            Item.noUseGraphic = false;
        }

        public override void HoldItem(Player player)
        {
            player.itemLocation.X = player.Center.X + (player.direction == 1 ? 20f : -20f);
            player.itemLocation.Y = player.Center.Y + 5f;
        }

        public override bool CanUseItem(Player player)
        {
            if (shotsFired >= MaxShots)
            {
                return false;
            }

            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            shotsFired++;

            if (shotsFired >= MaxShots)
            {
                player.itemTime = player.itemAnimation = 60;
                shotsFired = 0;
            }

            return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;
            position += muzzleOffset;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FrozenFragment>(), 15)
                .AddIngredient(ItemID.Revolver)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}