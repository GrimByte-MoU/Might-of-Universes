using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class LichChestplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaper) += 0.06f;
            player.GetCritChance(reaper) += 6f;

            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            if (acc.SoulCostMultiplier == 0f)
                acc.SoulCostMultiplier = 1f;
            acc.SoulCostMultiplier *= 0.95f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 20)
                .AddIngredient(ItemID.Bone, 30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}