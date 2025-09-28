using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class LifesTwilight : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 50f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 75;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EclipseRay>();
            Item.shootSpeed = 15f;
        }

        public override void HoldItem(Player player)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed && reaper.ConsumeSoulEnergy(SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost)))
            {
                IEntitySource src = player.GetSource_ItemUse(Item);
                int damage = player.GetWeaponDamage(Item);
                float kb = player.GetWeaponKnockback(Item);

                Projectile.NewProjectile(src, player.Center, Vector2.Zero,
                    ModContent.ProjectileType<ScytheEclipse>(),
                    damage * 2, kb, player.whoAmI, 0f);

                Projectile.NewProjectile(src, player.Center, Vector2.Zero,
                    ModContent.ProjectileType<ScytheEclipse>(),
                    damage * 2, kb, player.whoAmI, MathHelper.Pi);

                Projectile.NewProjectile(src, player.Center, Vector2.Zero,
                    ModContent.ProjectileType<ScytheEclipse>(),
                    damage * 2, kb, player.whoAmI, MathHelper.Pi);

                Projectile.NewProjectile(src, player.Center, Vector2.Zero,
                    ModContent.ProjectileType<ScytheEclipse>(),
                    damage * 2, kb, player.whoAmI, MathHelper.Pi);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(5f, target.Center);
            Dust.NewDust(target.position, target.width, target.height, DustID.OrangeTorch);
            Dust.NewDust(target.position, target.width, target.height, DustID.Torch);
            Lighting.AddLight(target.Center, 1f, 0.5f, 0f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Double-helix: exactly two projectiles, one per strand.
            float offset = 20f;
            Vector2 dir = Vector2.Normalize(velocity);
            Vector2 position1 = position + dir.RotatedBy(MathHelper.PiOver2) * offset;
            Vector2 position2 = position + dir.RotatedBy(-MathHelper.PiOver2) * offset;

            // Use opposing phase values so they spiral around each other.
            Projectile.NewProjectile(source, position1, velocity, type, damage, knockback, player.whoAmI, 0.5f);
            Projectile.NewProjectile(source, position2, velocity, type, damage, knockback, player.whoAmI, -0.5f);

            // Return false to prevent the default extra projectile so the total is two.
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<EclipseLight>(), 12)
                .AddIngredient(ModContent.ItemType<SolunarScythe>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}