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
    public class AdamantiteScythe : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 40f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 50;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AdamantiteScytheProjectile>();
            Item.shootSpeed = 10f;
        }
        public override void HoldItem(Player player)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                if (reaper.ConsumeSoulEnergy(effectiveCost))
                {
                    // Derive a velocity toward the cursor
                    Vector2 from = player.MountedCenter;
                    Vector2 dir = Main.MouseWorld - from;
                    if (dir.LengthSquared() < 0.0001f)
                        dir = new Vector2(player.direction, 0f);
                    dir.Normalize();
                    Vector2 velocity = dir * (Item.shootSpeed > 0 ? Item.shootSpeed : 10f);

                    // Use item-based damage/KB scaling
                    int damage = player.GetWeaponDamage(Item);
                    float kb = player.GetWeaponKnockback(Item);
                    IEntitySource src = player.GetSource_ItemUse(Item);

                    Projectile.NewProjectile(
                        src,
                        from,
                        velocity,
                        ModContent.ProjectileType<AdamantiteSphereProjectile>(),
                        damage * 3,
                        kb,
                        player.whoAmI
                    );
                }
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(0.8f, target.Center);

            if (!target.active)
            {
                reaper.AddSoulEnergy(0.8f, target.Center);
            }
            target.AddBuff(BuffID.Electrified, 180);
        }

        // Normal shooting – no release logic here anymore
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AdamantiteBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}