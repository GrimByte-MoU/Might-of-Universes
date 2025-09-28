using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class WardensHook : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 25f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 30;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<WardenBoltProjectile>();
            Item.shootSpeed = 16f;
        }

        public override void HoldItem(Player player)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed && reaper.ConsumeSoulEnergy(SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost)))
            {
                Vector2 from = player.MountedCenter;
                Vector2 dir = Main.MouseWorld - from;
                if (dir.LengthSquared() < 0.0001f) dir = new Vector2(player.direction, 0f);
                dir.Normalize();
                Vector2 velocity = dir * (Item.shootSpeed > 0 ? Item.shootSpeed : 16f);

                IEntitySource src = player.GetSource_ItemUse(Item);
                int damage = player.GetWeaponDamage(Item);
                float kb = player.GetWeaponKnockback(Item);

                Projectile.NewProjectile(
                    src,
                    from,
                    velocity,
                    ModContent.ProjectileType<WardenSpearProjectile>(),
                    damage * 2,
                    kb * 1.5f,
                    player.whoAmI
                );
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(2f, target.Center);

            Dust.NewDust(target.position, target.width, target.height, DustID.Water);
            Lighting.AddLight(target.Center, 0f, 0f, 1f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.rand.NextFloat() <= 0.15f)
            {
                Projectile.NewProjectile(
                    source,
                    position,
                    velocity,
                    ModContent.ProjectileType<WardenCrossProjectile>(),
                    (int)(damage * 1.25f),
                    knockback,
                    player.whoAmI
                );
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bone, 25)
                .AddIngredient(ItemID.WaterBolt)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}