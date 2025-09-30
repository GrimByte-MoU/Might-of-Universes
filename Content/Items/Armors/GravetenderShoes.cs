namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class GravetenderShoes : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 95);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.moveSpeed += 0.05f;
            player.GetDamage(reaper) += 0.03f;
            player.GetCritChance(reaper) += 3f;
            player.GetModPlayer<GravetenderSetPlayer>().poisonArmorPen += 3f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CactusLeggings, 1)
                .AddIngredient(ItemID.Vine, 2)
                .AddIngredient(ItemID.Stinger, 10)
                .AddIngredient(ItemID.JungleSpores, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}