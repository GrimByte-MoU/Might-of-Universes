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
    public class GlitchScythe : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 55f;

        private int buffTimer = 0;

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
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GlitchScytheProjectile>();
            Item.shootSpeed = 10f;
            Item.maxStack = 1;
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
                    Vector2 baseVel = dir * (Item.shootSpeed > 0 ? Item.shootSpeed : 10f);

                    IEntitySource src = player.GetSource_ItemUse(Item);
                    int damage = player.GetWeaponDamage(Item);
                    float kb = player.GetWeaponKnockback(Item);

                    for (int i = -1; i <= 1; i++)
                    {
                        Vector2 newVelocity = baseVel.RotatedBy(MathHelper.ToRadians(4 * i));
                        Projectile.NewProjectile(
                            src,
                            from,
                            newVelocity,
                            ModContent.ProjectileType<GlitchBlast>(),
                            damage,
                            kb,
                            player.whoAmI
                        );
                    }
                }
            }
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

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GlitchyChunk>(), 10)
                .AddIngredient(ModContent.ItemType<VaporFragment>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}