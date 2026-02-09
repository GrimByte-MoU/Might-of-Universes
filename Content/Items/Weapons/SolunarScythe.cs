using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class SolunarScythe : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 55f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 37;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 1, silver: 50);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SolunarProjectile>();
            Item.shootSpeed = 15f;
            Item.maxStack = 1;
        }

        public override void HoldItem(Player player)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            if (ReaperPlayer.SoulReleaseKey != null &&
                ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int cost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                if (!reaper.ConsumeSoulEnergy(cost))
                    return;

                IEntitySource src = player.GetSource_ItemUse(Item);
                int damage = player.GetWeaponDamage(Item);
                float kb = player.GetWeaponKnockback(Item);
                
                Projectile.NewProjectile(src, player.Center, Vector2.Zero,
                    ModContent.ProjectileType<SolunarMedallion>(), damage * 2, kb, player.whoAmI, 0f);

                Projectile.NewProjectile(src, player.Center, Vector2.Zero,
                    ModContent.ProjectileType<SolunarMedallion>(), damage * 2, kb, player.whoAmI, MathHelper.Pi);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 dir = velocity.SafeNormalize(Vector2.UnitX);
            float lateral = 12f;
            Vector2 perp = dir.RotatedBy(MathHelper.PiOver2) * lateral;

            Projectile.NewProjectile(source, position + perp, velocity, type, damage, knockback, player.whoAmI, 0f);
            Projectile.NewProjectile(source, position - perp, velocity, type, damage, knockback, player.whoAmI, MathHelper.Pi);

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SolunarToken>(), 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}