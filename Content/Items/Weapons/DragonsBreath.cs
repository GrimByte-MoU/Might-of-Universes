using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class DragonsBreath : ModItem
    {
        private int shotCounter = 0;

        public override void SetDefaults()
        {
            Item.damage = 150;
            Item.DamageType = DamageClass.Magic;
            Item.width = 50;
            Item.height = 40;
            Item.useTime = 6;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1f;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.shoot = ModContent.ProjectileType<DragonFlameDust>();
            Item.shootSpeed = 16f;
            Item.mana = 8;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item34;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);

            shotCounter++;

            if (shotCounter >= 10)
            {
                shotCounter = 0;
                
                float coneAngle = MathHelper.ToRadians(10);
                Vector2 sparkVelocity = velocity.RotatedByRandom(coneAngle) * 1.2f;
                
                Projectile.NewProjectile(source, position, sparkVelocity, ModContent.ProjectileType<Dragonspark>(), damage, knockback * 1.5f, player.whoAmI);
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpellTome)
                .AddIngredient(ItemID.Flamethrower)
                .AddIngredient(ModContent.ItemType<ChocolateScale>(), 7)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 7)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}