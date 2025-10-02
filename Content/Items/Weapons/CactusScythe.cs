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
    public class CactusScythe : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 40f;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 8;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(copper: 50);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CactusThorn>();
            Item.shootSpeed = 8f;
        }

        public override void HoldItem(Player player)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                if (reaper.ConsumeSoulEnergy(effectiveCost))
                {
                    Vector2 from = player.MountedCenter;
                    Vector2 dir = Main.MouseWorld - from;
                    if (dir.LengthSquared() < 0.0001f) dir = new Vector2(player.direction, 0f);
                    dir.Normalize();
                    Vector2 baseVel = dir * (Item.shootSpeed > 0 ? Item.shootSpeed : 8f);

                    IEntitySource src = player.GetSource_ItemUse(Item);
                    int damage = player.GetWeaponDamage(Item);
                    float kb = player.GetWeaponKnockback(Item);

                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 newVelocity = baseVel.RotatedBy(MathHelper.ToRadians(-10 + (i * 5)));
                        Projectile.NewProjectile(src, from, newVelocity, Item.shoot, damage * 2, kb, player.whoAmI);
                    }
                }
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(1f, target.Center);

            if (!target.active)
            {
                reaper.AddSoulEnergy(1f, target.Center);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cactus, 15)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}