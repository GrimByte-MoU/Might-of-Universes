namespace MightofUniverses.Content.Items.Accessories
{
    public class SoulCapacitor : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.maxSoulEnergy += 100;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wire, 25)
                .AddIngredient(ItemID.PalladiumBar, 5)
                .AddIngredient(ModContent.ItemType<SoulBattery>(), 1)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.Wire, 25)
                .AddIngredient(ItemID.CobaltBar, 5)
                .AddIngredient(ModContent.ItemType<SoulBattery>(), 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}