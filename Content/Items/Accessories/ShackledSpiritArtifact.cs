namespace MightofUniverses.Content.Items.Accessories
{
    public class ShackledSpiritArtifact : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28; Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 1, 50, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<ReaperDamageClass>() += 0.07f;
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.accShackledArtifact = true;
            acc.ApplyMaxSoulFromHP(0.075f);
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<SoulSiphoningArtifact>(), 1);
            r.AddIngredient(ItemID.SoulofLight, 5);
            r.AddIngredient(ItemID.SoulofNight, 5);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();
        }
    }
}