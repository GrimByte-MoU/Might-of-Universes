namespace MightofUniverses.Content.Items.Accessories
{
    public class SoulSiphoningArtifact : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            player.GetDamage<ReaperDamageClass>() += 0.05f;

            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.HasSoulSiphoningArtifact = true;
            acc.ApplyMaxSoulFromHP(0.05f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Diamond, 2)
                .AddIngredient(ItemID.TissueSample, 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.Diamond, 2)
                .AddIngredient(ItemID.RottenChunk, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}