using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class CrypticMessage : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.BonusEmpowerReaperDamage += 0.10f;
            acc.BonusEmpowerAttackSpeed += 0.05f;
            acc.BonusEmpowerCritChance += 5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<OminousNote>()
                .AddIngredient(ItemID.ShadowScale, 10)
                .AddIngredient(ItemID.HellstoneBar, 6)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient<OminousNote>()
                .AddIngredient(ItemID.TissueSample, 10)
                .AddIngredient(ItemID.HellstoneBar, 6)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}