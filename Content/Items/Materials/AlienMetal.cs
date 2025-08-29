using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Materials
{
    public class AlienMetal : ModItem
    {
        public override void SetStaticDefaults() 
        {
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.rare = ModContent.RarityType<StarsteelRarity>();
            Item.rare = ItemRarityID.Yellow;
        }
    }
}