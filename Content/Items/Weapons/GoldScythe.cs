using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Common.Abstractions;
using MightofUniverses.Common.Util;

namespace MightofUniverses.Content.Items.Weapons
{
    public class GoldScythe : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 40f;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 20;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(silver: 6);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player)
        {
            if (ReaperPlayer.SoulReleaseKey != null && ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(
                    player,
                    cost: effectiveCost,
                    durationTicks: 240,
                    configure: vals =>
                    {
                        vals.Defense += 5;
                        vals.ReaperDamage += 0.10f;
                    }
                );
                player.Heal(40);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(1f, target.Center);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoldBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}