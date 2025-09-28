using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class GreatBlizzard88 : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 50f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 80;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SnowflakeProjectile>();
            Item.shootSpeed = 14f;
        }

        public override void HoldItem(Player player)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();

            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed && reaper.ConsumeSoulEnergy(SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost)))
            {
                IEntitySource src = player.GetSource_ItemUse(Item);
                int damage = player.GetWeaponDamage(Item);
                float kb = player.GetWeaponKnockback(Item);

                for (int i = 0; i < 25; i++)
                {
                    Vector2 snowPosition = new Vector2(
                        player.Center.X + Main.rand.NextFloat(-400f, 400f),
                        player.Center.Y - Main.rand.NextFloat(600f, 800f)
                    );
                    Vector2 snowVelocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(12f, 16f));
                    Projectile.NewProjectile(
                        src,
                        snowPosition,
                        snowVelocity,
                        ModContent.ProjectileType<BlizzardSnowflakeProjectile>(),
                        damage / 2,
                        kb / 4f,
                        player.whoAmI
                    );
                }
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 180);
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(5f, target.Center);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-5f, 5f)));
                Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<FrozenFragment>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}