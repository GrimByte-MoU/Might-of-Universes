namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class GravetenderChestplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaper) += 0.04f;
            player.GetCritChance(reaper) += 4f;
            player.GetModPlayer<GravetenderSetPlayer>().poisonArmorPen += 4f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CactusBreastplate, 1)
                .AddIngredient(ItemID.Vine, 3)
                .AddIngredient(ItemID.Stinger, 13)
                .AddIngredient(ItemID.JungleSpores, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}