using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Materials;
using System.Collections.Generic;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Projectiles;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Ketsumatsu : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 180;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(0, 25, 0, 0);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;

            // needed to let Shoot() fire petals
            Item.shoot = ModContent.ProjectileType<KetsumatsuPetal>();
            Item.shootSpeed = 10f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 5)
                .AddIngredient(ItemID.LifeCrystal, 5)
                .AddIngredient(ModContent.ItemType<OrichalcumScythe>(), 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            // --- Soul Release ability ---
            if (ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                if (reaper.ConsumeSoulEnergy(125f))
                {
                    // Sakura Bloom appears at cursor
                    Vector2 cursorPos = Main.MouseWorld;
                    Projectile.NewProjectile(
                        source,
                        cursorPos,
                        Vector2.Zero,
                        ModContent.ProjectileType<KetsumatsuBloom>(),
                        (int)(damage * 3f),
                        knockback,
                        player.whoAmI
                    );

                    // pink pixel line to cursor
                    for (int i = 0; i < 50; i++)
                    {
                        float t = i / 50f;
                        Vector2 dustPos = Vector2.Lerp(player.Center, cursorPos, t);
                        Dust.NewDustPerfect(dustPos, DustID.PinkCrystalShard, Vector2.Zero, 150, Color.LightPink, 1.2f).noGravity = true;
                    }

                    return false; // stop normal petals this swing
                }
                else
                {
                    Main.NewText("Not enough soul energy to activate!", Color.Red);
                }
            }

            // --- Normal swing petals ---
            int numPetals = Main.rand.NextBool() ? 1 : 2; // randomly 1 or 2
            for (int i = 0; i < numPetals; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(3));
                Projectile.NewProjectile(
                    source,
                    position,
                    perturbedSpeed * 1.2f,
                    ModContent.ProjectileType<KetsumatsuPetal>(),
                    damage,
                    knockback,
                    player.whoAmI
                );
            }

            return false; // no vanilla projectile, just our petals
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(10f, target.Center);
        }
    }
}

