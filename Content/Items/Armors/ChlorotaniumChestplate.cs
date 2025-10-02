using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class ChlorotaniumChestplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.defense = 20;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(gold: 11);
        }

        public override void UpdateEquip(Player player)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaperClass) += 0.05f;
            player.GetCritChance(reaperClass) += 5f;
            player.statLifeMax2 += 10;

            player.GetModPlayer<ChlorotaniumSetPlayer>().HasChloroChest = true;
        }

        public override void AddRecipes()
        {
            // Titanium variant
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 24)
                .AddIngredient(ItemID.TitaniumBar, 24)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            // Adamantite variant
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 24)
                .AddIngredient(ItemID.AdamantiteBar, 24)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}