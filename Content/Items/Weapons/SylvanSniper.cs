using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Ammo;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Weapons
{
    public class SylvanSniper : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 350;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 88;
            Item.height = 28;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 8f;
            Item.value = Item.sellPrice(platinum: 1, gold: 50);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.shootSpeed = 20f;
            Item.shoot = ProjectileID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
            Item.UseSound = SoundID.Item40;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.scale = 1.1f;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            int bulletDamage = GetBulletBaseDamage(type);
            int leafDamage = (int)(bulletDamage * 1.5f);

            Projectile.NewProjectile(
                source,
                position,
                velocity,
                ModContent.ProjectileType<SharpLeaf>(),
                damage + leafDamage,
                knockback,
                player.whoAmI
            );

            return false;
        }

        private int GetBulletBaseDamage(int projectileType)
        {
        if (projectileType == ModContent.ProjectileType<CodeBulletProjectile>())
        return 18;
        if (projectileType == ModContent.ProjectileType<EclipseRoundProjectile>())
        return 22;
        if (projectileType == ModContent.ProjectileType<FearsomeRoundProjectile>())
        return 18;
        if (projectileType == ModContent.ProjectileType<FrozenBulletProjectile>())
        return 22;
        if (projectileType == ModContent.ProjectileType<HollowPointProjectile>())
        return 27;
        if (projectileType == ModContent.ProjectileType<LightRoundProjectile>())
        return 30;
        if (projectileType == ModContent.ProjectileType<SanguineBulletProjectile>())
        return 24;
    
            return projectileType switch
            {
                ProjectileID.BulletHighVelocity => 17,
                ProjectileID.Bullet => 10,
                ProjectileID.ExplosiveBullet => 15,
                ProjectileID.MeteorShot => 12,
                ProjectileID.PartyBullet => 15,
                ProjectileID.NanoBullet => 23,
                ProjectileID.SilverBullet => 13,
                ProjectileID.GoldenBullet => 15,
                ProjectileID.CrystalBullet => 13,
                ProjectileID.CursedBullet => 18,
                ProjectileID.IchorBullet => 20,
                ProjectileID.ChlorophyteBullet => 13,
                ProjectileID.MoonlordBullet => 30,
                ProjectileID.VenomBullet => 23,


            };
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SniperRifle, 1)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 8)
                .AddIngredient(ItemID.JungleSpores, 10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}