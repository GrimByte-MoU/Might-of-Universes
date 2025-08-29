using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class ClockworkMinigun : ModItem
    {
        private float speedPenalty = 0f;
        private const float MAX_SPEED_PENALTY = 0.5f;
        private const float PENALTY_RATE = 0.1f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 26;
            Item.rare = ItemRarityID.Lime;
            Item.useTime = 3;
            Item.useAnimation = 3;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item11;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 35;
            Item.knockBack = 1f;
            Item.noMelee = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 32f;
            Item.useAmmo = AmmoID.Bullet;
            Item.value = Item.sellPrice(gold: 8);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, 2f);
        }

        public override void UpdateInventory(Player player)
        {
            if (player.itemAnimation > 0 && Item.type == player.HeldItem.type)
            {
                speedPenalty = System.Math.Min(speedPenalty + PENALTY_RATE, MAX_SPEED_PENALTY);
                player.moveSpeed *= (1f - speedPenalty);
            }
            else
            {
                speedPenalty = 0f;
            }
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)

        {
            Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
            Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI, 1);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BrassBar>(), 12)
                .AddIngredient(ItemID.Wire, 50)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}

