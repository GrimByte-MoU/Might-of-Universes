namespace MightofUniverses.Content.Items.Accessories
{
    public class SoulBattery : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.maxSoulEnergy += 50;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 15)
                .AddIngredient(ItemID.CopperBar, 10)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.LeadBar, 15)
                .AddIngredient(ItemID.TinBar, 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}