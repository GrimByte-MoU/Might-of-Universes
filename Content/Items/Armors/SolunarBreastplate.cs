using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class SolunarBreastplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 9;
        }

        public override void UpdateEquip(Player player)
        {
            ReaperPlayer reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.UpdateReaperDamageMultiplier(0.1f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SolunarToken>(), 20)
                .AddIngredient(ModContent.ItemType<DesertWrappings>(), 20)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
