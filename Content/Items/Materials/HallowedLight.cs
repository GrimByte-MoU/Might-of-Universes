namespace MightofUniverses.Content.Items.Materials
{
    public class HallowedLight : ModItem
    {
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
