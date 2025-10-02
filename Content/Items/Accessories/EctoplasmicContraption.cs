using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class EctoplasmicContraption : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 20);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.maxSoulEnergy += 200;

            var acc = player.GetModPlayer<ReaperAccessoryPlayer>(); 
            acc.SoulCostMultiplier *= 0.90f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 10)
                .AddIngredient(ModContent.ItemType<LunaticCloth>(), 5)
                .AddIngredient(ItemID.Ectoplasm, 25)
                .AddIngredient(ItemID.SpectreBar, 10)
                .AddIngredient(ModContent.ItemType<OverloadedSoulPack>(), 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}