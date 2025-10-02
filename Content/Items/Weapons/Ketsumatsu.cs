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
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Ketsumatsu : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 200f;

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
            Item.shoot = ModContent.ProjectileType<KetsumatsuPetal>();
            Item.shootSpeed = 10f;
        }

        public override void HoldItem(Player player)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed && reaper.ConsumeSoulEnergy(SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost)))
            {
                IEntitySource src = player.GetSource_ItemUse(Item);
                int damage = player.GetWeaponDamage(Item);
                float kb = player.GetWeaponKnockback(Item);

                Vector2 cursorPos = Main.MouseWorld;
                Projectile.NewProjectile(
                    src,
                    cursorPos,
                    Vector2.Zero,
                    ModContent.ProjectileType<KetsumatsuBloom>(),
                    (int)(damage * 4f),
                    kb,
                    player.whoAmI
                );

                for (int i = 0; i < 50; i++)
                {
                    float t = i / 50f;
                    Vector2 dustPos = Vector2.Lerp(player.Center, cursorPos, t);
                    Dust.NewDustPerfect(dustPos, DustID.PinkCrystalShard, Vector2.Zero, 150, Color.LightPink, 1.2f).noGravity = true;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 5)
                .AddIngredient(ItemID.LifeCrystal, 5)
                .AddIngredient(ModContent.ItemType<OrichalcumScythe>(), 1)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numPetals = Main.rand.NextBool() ? 1 : 2;
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

            return false;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(10f, target.Center);
        }
    }
}