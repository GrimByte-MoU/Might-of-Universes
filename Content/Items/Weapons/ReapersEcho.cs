using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class ReapersEcho : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 200f;
        private int mirageSpawnTimer = 0;
        private const int MIRAGE_SPAWN_RATE = 10;

        public override void SetDefaults()
        {
            Item.width = 80;
            Item.height = 80;
            Item.damage = 200;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 15);
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = false;
        }

        public override void HoldItem(Player player)
        {
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();

            if (player.itemAnimation > 0)
            {
                mirageSpawnTimer++;
                if (mirageSpawnTimer >= MIRAGE_SPAWN_RATE)
                {
                    mirageSpawnTimer = 0;
                    SpawnMirage(player);
                }
            }
            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                if (reaperPlayer.ConsumeSoulEnergy(effectiveCost))
                {
                    SpawnUltimateMirages(player);
                    SoundEngine.PlaySound(SoundID.Item84, player.Center);
                }
            }
        }

        private void SpawnMirage(Player player)
        {
            Vector2 direction = Main.MouseWorld - player.Center;
            direction.Normalize();

            int damage = player.GetWeaponDamage(Item);
            float kb = player.GetWeaponKnockback(Item);

            Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, direction * 20f,
                ModContent.ProjectileType<ReapersEchoMirage>(), damage, kb, player.whoAmI, 0f);
        }

        private void SpawnUltimateMirages(Player player)
        {
            int damage = player.GetWeaponDamage(Item);
            float kb = player.GetWeaponKnockback(Item);

            for (int i = 0; i < 8; i++)
            {
                float angle = MathHelper.TwoPi / 8f * i;
                Vector2 offset = angle.ToRotationVector2() * 120f;
                Vector2 spawnPos = player.Center + offset;

                Projectile.NewProjectile(player.GetSource_ItemUse(Item), spawnPos, Vector2.Zero,
                    ModContent.ProjectileType<ReapersEchoUltimateMirage>(), (int)(damage * 3f), kb * 2f, player.whoAmI, 0f);
            }
        }

        public override void AddRecipes()
{
    CreateRecipe()
        .AddIngredient(ModContent.ItemType<ChlorophyteScythe>(), 1)
        .AddIngredient(ModContent.ItemType<WardensHook>(), 1)
        .AddIngredient(ModContent.ItemType<CriticalFailure>(), 1)
        .AddIngredient(ModContent.ItemType<Orcus>(), 1)
        .AddIngredient(ModContent.ItemType<TempleReaper>(), 1)
        .AddIngredient(ModContent.ItemType<NewMoon>(), 1)
        .AddIngredient(ModContent.ItemType<CelestialReaper>(), 1)
        .AddIngredient(ModContent.ItemType<MidnightsReap>(), 1)
        .AddIngredient(ModContent.ItemType<CactusScythe>(), 1)
        .AddIngredient(ModContent.ItemType<IndustrialSeverence>(), 1)
        .AddTile(TileID.LunarCraftingStation)
        .Register();
}
    }
}