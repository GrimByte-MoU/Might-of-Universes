using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using Terraria.DataStructures;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Weapons
{
    public class GummyScythe : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 50;
            Item.damage = 17;
            Item.DamageType = ModContent.GetInstance<ReaperDamageClass>();
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4.5f;
            Item.value = Item.sellPrice(silver: 8);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.AddSoulEnergy(1f, target.Center);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Use new centralized empowerment system for instant heal + Hyper buff
            bool released = ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(
                player, 30f, 1, // Very short duration since this is instant effect only
                new ReaperEmpowermentValues(), // No ongoing empowerment
                onConsume: (p) => {
                    // Apply instant healing and Hyper buff when souls are consumed
                    p.Heal(50);
                    p.AddBuff(ModContent.BuffType<Hyper>(), 180); // Hyper buff for 3 seconds
                });
            
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
               .AddIngredient(ModContent.ItemType<SweetTooth>(), 5)
                .AddIngredient(ModContent.ItemType<GummyMembrane>(), 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
