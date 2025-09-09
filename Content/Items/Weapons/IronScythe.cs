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
    public class IronScythe : ModItem, IHasSoulCost
    {
        public float BaseSoulCost => 30f;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 12;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(silver: 4);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            player.GetModPlayer<ReaperPlayer>().AddSoulEnergy(1f, target.Center);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ReaperPlayer.SoulReleaseKey.JustPressed)
            {
                int effectiveCost = SoulCostHelper.ComputeEffectiveSoulCostInt(player, BaseSoulCost);
                ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(
                    player,
                    cost: effectiveCost,
                    durationTicks: 180,
                    configure: vals => { vals.LifeRegen += 5; }
                );
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}