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
    public class CriticalFailure : ModItem
    {
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
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CriticalFailureProjectile>();
            Item.shootSpeed = 15f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(5f, target.Center);
            if (!target.active)
                player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(5f, target.Center);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                if (ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(
                    player,
                    cost: 30f,
                    durationTicks: 60, // brief visual window; adjust or set to 0 if undesired
                    configure: vals => { }
                ))
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(10 * i));
                        Projectile.NewProjectile(
                            source,
                            position,
                            newVelocity,
                            ModContent.ProjectileType<CodeBolt>(),
                            damage * 2,
                            knockback * 1.5f,
                            player.whoAmI
                        );
                    }
                }
                return false;
            }
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