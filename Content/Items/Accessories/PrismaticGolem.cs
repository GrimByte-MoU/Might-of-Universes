using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories;
public class PrismaticGolem : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.value = Item.sellPrice(gold: 10);
        Item.rare = ItemRarityID.Yellow;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.endurance += 0.1f;
          player.lifeRegen += 4;
            player.statLifeMax2 += 30;
        player.GetModPlayer<FleshGolemPlayer>().hasPrismaticGolem = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<FleshGolem>())
            .AddIngredient(ModContent.ItemType<PrismaticCatalyst>(), 3)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}
