using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class YinYangShoes : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.moveSpeed += 0.10f;
            player.GetDamage(reaper) += 0.03f;
            player.GetCritChance(reaper) += 3f;
            player.statLifeMax2 += 10;
            player.endurance += 0.02f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                   .AddIngredient(ItemID.SoulofLight, 3)
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddIngredient(ItemID.LightShard, 1)
                .AddIngredient(ItemID.DarkShard, 1)
                .AddIngredient(ItemID.CrystalShard, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}