using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class DybbukBox : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.BonusEmpowerReaperDamage += 0.15f;
            acc.BonusEmpowerAttackSpeed += 0.10f;
            acc.BonusEmpowerCritChance += 12f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<OuijaBoard>()
                .AddIngredient(ModContent.ItemType<PureTerror>(), 5)
                .AddIngredient(ItemID.SpectreBar, 7)
                .AddIngredient(ItemID.GuideVoodooDoll)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}