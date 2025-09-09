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

namespace MightofUniverses.Content.Items.Weapons
{
    public class ChlorotaniumScythe : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 200f;

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 70;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ChlorotaniumStar>();
            Item.shootSpeed = 10f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(5f, target.Center);
            if (!target.active)
                reaper.AddSoulEnergy(5f, target.Center);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                bool released = ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(
                    player,
                    cost: BaseSoulCost,
                    durationTicks: 300,
                    configure: vals =>
                    {
                        vals.Defense += 20;
                        vals.Endurance += 0.15f;
                        vals.LifeRegen += 15;
                    }
                );
                if (released)
                    player.Heal(150);

                return true; // keep default firing behavior
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ChlorophyteScythe>())
                .AddIngredient(ModContent.ItemType<TitaniumScythe>())
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ChlorophyteScythe>())
                .AddIngredient(ModContent.ItemType<AdamantiteScythe>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}