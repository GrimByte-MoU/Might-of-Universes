namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class YinYangCloak : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaper) += 0.04f;
            player.GetCritChance(reaper) += 4f;
            player.statLifeMax2 += 10;
            player.endurance += 0.02f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                 .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.LightShard, 1)
                .AddIngredient(ItemID.DarkShard, 1)
                .AddIngredient(ItemID.CrystalShard, 25)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}