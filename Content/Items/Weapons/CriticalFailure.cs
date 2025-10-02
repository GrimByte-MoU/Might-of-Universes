using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class CriticalFailure : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 45f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 60;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CriticalFailureProjectile>();
            Item.shootSpeed = 15f;
        }

        public override void HoldItem(Player player)
        {
            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int cost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                if (ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(player, cost: cost, durationTicks: 60, configure: _ => { }))
                {
                    Vector2 from = player.MountedCenter;
                    Vector2 dir = Main.MouseWorld - from;
                    if (dir.LengthSquared() < 0.0001f) dir = new Vector2(player.direction, 0f);
                    dir.Normalize();
                    Vector2 baseVel = dir * (Item.shootSpeed > 0 ? Item.shootSpeed : 15f);

                    IEntitySource src = player.GetSource_ItemUse(Item);
                    int damage = player.GetWeaponDamage(Item);
                    float kb = player.GetWeaponKnockback(Item);

                    for (int i = 0; i <= 1; i++)
                    {
                        Vector2 newVelocity = baseVel.RotatedBy(MathHelper.ToRadians(10 * i));
                        Projectile.NewProjectile(
                            src,
                            from,
                            newVelocity,
                            ModContent.ProjectileType<CodeBolt>(),
                            damage * 5,
                            kb * 1.5f,
                            player.whoAmI
                        );
                    }
                }
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(5f, target.Center);
            if (!target.active)
                player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(5f, target.Center);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GlitchScythe>())
                .AddIngredient(ModContent.ItemType<ViralCode>(), 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}