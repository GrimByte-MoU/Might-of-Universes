using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using Terraria.Audio;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class SanguineSong : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 84;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(gold: 5);
            
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.autoReuse = true;
            Item.useAmmo = AmmoID.Arrow;
            
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 48;
            Item.knockBack = 3f;
            Item.crit = 4;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.noMelee = true;
        }

        public override void HoldItem(Player player)
        {
            Lighting.AddLight(player.Center, 1f, 0.1f, 0.1f);
            
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Blood, 0f, 2f, 0, default, 1f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity);
Vector2 shootPosition = position + muzzleOffset;

            int arrowCount = Main.rand.Next(1, 3);
            for (int i = 0; i < arrowCount; i++)
            {
                Vector2 arrowVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
                Projectile.NewProjectile(source, shootPosition, arrowVelocity, type, damage, knockback, player.whoAmI);
            }

            float spreadAngle = 15f;
            float startAngle = -spreadAngle / 2f;
            float angleStep = spreadAngle / 3f;

            for (int i = 0; i < 4; i++)
            {
                Vector2 rayVelocity = velocity.RotatedBy(MathHelper.ToRadians(startAngle + (angleStep * i)));
                rayVelocity *= 1f;
                
                Projectile.NewProjectile(source, shootPosition, rayVelocity, 
                    ModContent.ProjectileType<BloodRay>(), (int)(damage * 1.5f), knockback, player.whoAmI);
            }

            return false;
        }

        public override void UseItemFrame(Player player)
        {
            SoundEngine.PlaySound(SoundID.NPCHit13, player.position);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 15)
                .AddIngredient(ItemID.WoodenBow, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}


