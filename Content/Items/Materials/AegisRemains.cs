using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Materials
{
    public class AegisRemains : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.placeStyle = 0;
        }
    }
}
