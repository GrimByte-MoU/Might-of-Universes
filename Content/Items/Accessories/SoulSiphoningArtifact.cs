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
            reaper.reaperDamageMultiplier += 0.05f;

            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.HasSoulSiphoningArtifact = true;
            acc.ApplyMaxSoulFromHP(0.05f);
        }
    }
}