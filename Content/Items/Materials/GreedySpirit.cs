using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Materials
{
    public class GreedySpirit : ModItem
    {
        public override void SetStaticDefaults() 
        {
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 25);
            Item.rare = ItemRarityID.Yellow;
        }
    }
}
