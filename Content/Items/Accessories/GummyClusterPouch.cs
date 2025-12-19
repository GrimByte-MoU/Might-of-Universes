using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class GummyClusterPouch : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.value = Item.sellPrice(gold: 3);
        Item.rare = ItemRarityID.Green;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<SnackPouchPlayer>().hasGummyClusterPouch = true;
        player.GetModPlayer<ReaperPlayer>();
        player.GetDamage<ReaperDamageClass>() += 0.07f;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<SnackPouch>())
            .AddIngredient(ModContent.ItemType<SweetgumBar>(), 10)
            .AddIngredient(ModContent.ItemType<SugarClot>(), 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
}

}
