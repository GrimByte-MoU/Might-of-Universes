using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Weapons
{
    public class Aokigahara : ModItem, IHasSoulCost, IScytheWeapon
    {
        public float BaseSoulCost => 275f;

        public override void SetDefaults()
        {
            Item.width = 70;
            Item.height = 70;
            Item.damage = 145;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AokigaharaLeaf>();
            Item.shootSpeed = 14f;
            Item.maxStack = 1;
            Item.scale = 1.2f;
        }

        public override void HoldItem(Player player)
        {
            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(
                    player,
                    cost: BaseSoulCost,
                    durationTicks: 240,
                    configure: vals =>
                    {
                        vals.Defense = 20;
                        vals.ReaperDamage = 0.30f;
                        vals.CritChance = 25f;
                        vals.LifeRegen = 20;
                    }
                );
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(5f, target.Center);

            target.AddBuff(ModContent.BuffType<NaturesToxin>(), 180);

            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    target.position,
                    target.width,
                    target.height,
                    DustID.PinkFairy,
                    Main.rand.NextFloat(-3f, 3f),
                    Main.rand.NextFloat(-3f, 3f),
                    100,
                    Color.Fuchsia,
                    1.5f
                );
                dust.noGravity = true;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 baseDirection = velocity;
            baseDirection.Normalize();

            Vector2 leftVelocity = baseDirection.RotatedBy(MathHelper.ToRadians(-15)) * Item.shootSpeed;
            Projectile.NewProjectile(
                source,
                position,
                leftVelocity,
                type,
                damage / 2,
                knockback * 0.5f,
                player.whoAmI
            );

            Vector2 rightVelocity = baseDirection.RotatedBy(MathHelper.ToRadians(15)) * Item.shootSpeed;
            Projectile.NewProjectile(
                source,
                position,
                rightVelocity,
                type,
                damage / 2,
                knockback * 0.5f,
                player.whoAmI
            );

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.JungleSpores, 10)
                .AddIngredient(ItemID.ChlorophyteBar, 7)
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}