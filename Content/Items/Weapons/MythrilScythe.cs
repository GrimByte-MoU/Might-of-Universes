using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using Terraria.DataStructures;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class MythrilScythe : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 40f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 44;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 1, silver: 50);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MythrilScytheProjectile>();
            Item.shootSpeed = 10f;
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
            var reaper = player.GetModPlayer<ReaperPlayer>();

            if (ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                if (reaper.ConsumeSoulEnergy(effectiveCost))
                {
                    Projectile.NewProjectile(
                        source,
                        position,
                        velocity,
                        ModContent.ProjectileType<MineralWaveProjectile>(),
                        damage,
                        knockback * 5f,
                        player.whoAmI
                    );
                    return false;
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MythrilBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}