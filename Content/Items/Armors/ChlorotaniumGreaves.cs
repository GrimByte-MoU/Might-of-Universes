using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class ChlorotaniumGreaves : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 18;
            Item.defense = 17;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(gold: 10);
        }

        public override void UpdateEquip(Player player)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaperClass) += 0.05f;
            player.GetCritChance(reaperClass) += 5f;
            player.statLifeMax2 += 10;
            player.moveSpeed += 0.13f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 18)
                .AddIngredient(ItemID.TitaniumBar, 18)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 18)
                .AddIngredient(ItemID.AdamantiteBar, 18)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}