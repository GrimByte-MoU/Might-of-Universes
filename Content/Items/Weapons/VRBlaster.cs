using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;
using System;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Weapons
{
    public class VRBlaster : ModItem
    {
        private bool rapidFire = false;
        private readonly int normalUseTime = 22;

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 20;
            Item.damage = 45;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = normalUseTime;
            Item.useAnimation = normalUseTime;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numShots = 1;
            if (Main.rand.NextFloat() < 0.20f)
                numShots = 2;
            if (Main.rand.NextFloat() < 0.10f)
                numShots = 3;

            for (int i = 0; i < numShots; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));
                Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);
            }

            if (Main.rand.NextFloat() < 0.05f)
            {
                Item.useTime = 1;
                Item.useAnimation = 1;
                rapidFire = true;
            }
            else if (rapidFire)
            {
                Item.useTime = normalUseTime;
                Item.useAnimation = normalUseTime;
                rapidFire = false;
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SyntheticumBar>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
