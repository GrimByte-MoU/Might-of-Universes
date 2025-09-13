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
    public class CobaltScythe : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 40f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 40;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(silver: 75);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CobaltScytheProjectile>();
            Item.shootSpeed = 8f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(3f, target.Center);

            if (!target.active)
            {
                reaper.AddSoulEnergy(3f, target.Center);
            }
        }

        public override void HoldItem(Player player)
        {
            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                var reaper = player.GetModPlayer<ReaperPlayer>();
                int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                if (reaper.ConsumeSoulEnergy(effectiveCost))
                {
                    Vector2 toMouse = Vector2.Normalize(Main.MouseWorld - player.MountedCenter);
                    Vector2 velocity = toMouse * Item.shootSpeed;
                    int damage = player.GetWeaponDamage(Item);
                    float knockback = player.GetWeaponKnockback(Item);
                    var source = player.GetSource_ItemUse(Item);

                    for (int i = -1; i <= 2; i++)
                    {
                        Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(10 * i));
                        Projectile.NewProjectile(
                            source,
                            player.MountedCenter,
                            newVelocity,
                            ModContent.ProjectileType<CobaltShotProjectile>(),
                            damage * 2,
                            knockback,
                            player.whoAmI
                        );
                    }
                }
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CobaltBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}