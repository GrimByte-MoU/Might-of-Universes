using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class OverloadedSoulPack : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 6);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.maxSoulEnergy += 150;

            var acc = player.GetModPlayer<ReaperAccessoryPlayer>(); 
            acc.SoulCostMultiplier *= 0.95f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofLight, 10)
                .AddIngredient(ItemID.SoulofNight, 10)
                .AddIngredient(ModContent.ItemType<GreedySpirit>(), 10)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ItemID.Ectoplasm, 10)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddIngredient(ItemID.ChlorophyteBar, 10)
                .AddIngredient(ModContent.ItemType<SoulCapacitor>(), 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}