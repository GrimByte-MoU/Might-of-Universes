using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Projectiles;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class SweetHarvester : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 75f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 22;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HarvesterGumballProjectile>();
            Item.shootSpeed = 14f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(2f, target.Center);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(
                    player,
                    effectiveCost,
                    300,
                    vals => { vals.LifeRegen += 10; }
                );
                player.Heal(100);
                player.AddBuff(ModContent.BuffType<Hyper>(), 300);
                return false;
            }

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GummyScythe>()
                .AddIngredient<SweetgumBar>(10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}