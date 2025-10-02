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
    public class LifesTwilight : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 65f;

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
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<EclipseRay>();
            Item.shootSpeed = 15f;
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

                // Four orbiters at fixed world anchor: phases 0, π/2, π, 3π/2
                float[] phases = {
                    0f,
                    MathHelper.PiOver2,
                    MathHelper.Pi,
                    MathHelper.Pi + MathHelper.PiOver2
                };

                foreach (float phase in phases)
                {
                    Projectile.NewProjectile(
                        src,
                        player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<ScytheEclipse>(), // Make sure this matches the file below
                        damage * 2,
                        kb,
                        player.whoAmI,
                        ai0: phase
                    );
                }

                // Optional debug:
                // Main.NewText("Life's Twilight ability spawned 4 eclipses.", Microsoft.Xna.Framework.Color.Orange);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Two helix strands: phases 0 & π (NOT ±0.5f)
            Vector2 dir = velocity.SafeNormalize(Vector2.UnitX);
            float lateral = 14f; // narrower than old 20 to reduce ground hits

            Vector2 perp = dir.RotatedBy(MathHelper.PiOver2) * lateral;

            Projectile.NewProjectile(source, position + perp, velocity, type, damage, knockback, player.whoAmI, 0f);
            Projectile.NewProjectile(source, position - perp, velocity, type, damage, knockback, player.whoAmI, MathHelper.Pi);

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