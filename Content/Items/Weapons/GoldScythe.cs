using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using Terraria.DataStructures;

namespace MightofUniverses.Content.Items.Weapons
{
    public class GoldScythe : ModItem
    {

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
    // Use new centralized empowerment system - instant heal + defense + damage boost
    var empowermentValues = ReaperSoulEffects.CreateDefenseAndDamageEmpowerment(5, 0.1f);
    bool released = ReaperSoulEffects.TryReleaseSoulsWithEmpowerment(player, 40f, 240, empowermentValues,
        onConsume: (p) => {
            // Apply instant healing when souls are consumed
            ReaperSoulEffects.ApplyInstantHealing(p, 40);
        });
    
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
