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
    public class DemonsFinger : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 40f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 55;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DemonsFingerProjectile>();
            Item.shootSpeed = 16f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(4f, target.Center);
            target.AddBuff(BuffID.OnFire3, 180);

            Dust.NewDust(target.position, target.width, target.height, DustID.Torch);
            Lighting.AddLight(target.Center, 1f, 0.3f, 0f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            if (ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                if (reaper.ConsumeSoulEnergy(effectiveCost))
                {
                    for (int i = 0; i < 7; i++)
                    {
                        Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-7.5f, 7.5f)));
                        Projectile.NewProjectile(
                            source,
                            position,
                            newVelocity * 1.5f,
                            ModContent.ProjectileType<Demonsoul>(),
                            (int)(damage * 1.5f),
                            knockback,
                            player.whoAmI,
                            0f,
                            i * 5
                        );
                    }
                    return false;
                }
            }

            position.Y -= Item.height / 2;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HellsTalon>())
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}