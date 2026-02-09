using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class IncantationInkEye : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.width = 28;
            Item.height = 34;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.knockBack = 1f;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<InkEyeMinion>();
            Item.buffType = ModContent.BuffType<InkEyeBuff>();
            Item.shootSpeed = 0f;
            Item.autoReuse = true;
            Item.maxStack = 1;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[type] > 0)
            {
                player.AddBuff(Item.buffType, 2);
                return false;
            }

            player.AddBuff(Item.buffType, 2);
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShadowScale, 5)
                .AddIngredient(ItemID.HellstoneBar, 7)
                .AddIngredient(ModContent.ItemType<Bonemeal>(), 12)
                .AddIngredient(ModContent.ItemType<DesertWrappings>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}