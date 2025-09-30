namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class GravetenderHat : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 90);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaper) += 0.03f;
            player.GetCritChance(reaper) += 3f;
            player.GetModPlayer<GravetenderSetPlayer>().poisonArmorPen += 3f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CactusHelmet, 1)
                .AddIngredient(ItemID.Vine, 1)
                .AddIngredient(ItemID.Stinger, 7)
                .AddIngredient(ItemID.JungleSpores, 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}