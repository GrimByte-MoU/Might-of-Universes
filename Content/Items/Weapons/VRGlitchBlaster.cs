using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;
using System;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Weapons
{
    public class VRGlitchBlaster : ModItem
    {
        private bool rapidFire = false;
        private int rapidFireTimer = 0;
        private const int RAPID_FIRE_DURATION = 30; // 1/2 second
        private const int NORMAL_USE_TIME = 16;

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 20;
            Item.damage = 45;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = NORMAL_USE_TIME;
            Item.useAnimation = NORMAL_USE_TIME;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Bullet;
            Item.maxStack = 1;
        }

        public override void UpdateInventory(Player player)
        {
            if (rapidFire)
            {
                rapidFireTimer++;
                if (rapidFireTimer >= RAPID_FIRE_DURATION)
                {
                    Item.useTime = NORMAL_USE_TIME;
                    Item.useAnimation = NORMAL_USE_TIME;
                    rapidFire = false;
                    rapidFireTimer = 0;
                }
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numShots = 2;
            float rand = Main.rand.NextFloat();
            
            if (rand < 0.30f) numShots = 3;
            else if (rand < 0.20f) numShots = 4;
            else if (rand < 0.05f) numShots = 5;

            for (int i = 0; i < numShots; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(2));
                Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);
            }

            if (Main.rand.NextFloat() < 0.05f && !rapidFire)
            {
                Item.useTime = 1;
                Item.useAnimation = 1;
                rapidFire = true;
                rapidFireTimer = 0;
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<VRBlaster>())
                .AddIngredient(ItemID.ChlorophyteBar, 5)
                .AddIngredient(ModContent.ItemType<GlitchyChunk>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
