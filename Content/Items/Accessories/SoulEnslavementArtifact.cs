namespace MightofUniverses.Content.Items.Accessories
{
    public class SoulEnslavementArtifact : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28; Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 3, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<ReaperDamageClass>() += 0.10f;
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.accSoulEnslavementArtifact = true;
            acc.ApplyMaxSoulFromHP(0.1f);
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<ShackledSpiritArtifact>(), 1);
            r.AddIngredient(ItemID.SoulofFright, 5);
            r.AddIngredient(ItemID.HallowedBar, 5);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();
        }
    }
}