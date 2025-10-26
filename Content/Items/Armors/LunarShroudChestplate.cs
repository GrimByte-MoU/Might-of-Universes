using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class LunarShroudChestplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ItemRarityID.Purple;
            Item.defense = 25;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetCritChance(reaper) += 8f;
            player.endurance += 0.05f;
            player.GetAttackSpeed(reaper) += 0.1f;

            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            if (acc.SoulCostMultiplier == 0f)
                acc.SoulCostMultiplier = 1f;
            acc.SoulCostMultiplier *= 0.9f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LunaticCloth>(), 20)
                .AddIngredient(ItemID.LunarBar, 12)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}