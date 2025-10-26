using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class LunarShroudCowl : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ItemRarityID.Purple;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaper) += 0.2f;
            player.GetCritChance(reaper) += 8f;
            player.endurance += 0.02f;
            player.GetAttackSpeed(reaper) += 0.1f;

            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            if (acc.SoulCostMultiplier == 0f)
                acc.SoulCostMultiplier = 1f;
            acc.SoulCostMultiplier *= 0.95f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LunaticCloth>(), 10)
                .AddIngredient(ItemID.LunarBar, 8)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}