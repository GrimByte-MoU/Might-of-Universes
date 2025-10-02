using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class MechaCactusLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.defense = 16;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 7);
        }

        public override void UpdateEquip(Player player)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaperClass) += 0.03f;
            player.GetCritChance(reaperClass) += 3f;
            player.statLifeMax2 += 10;
            player.endurance += 0.02f;
            player.moveSpeed += 0.10f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CactusLeggings, 1)
                .AddIngredient(ModContent.ItemType<Kevlar>(), 9)
                .AddIngredient(ModContent.ItemType<CarbonFiber>(), 12)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}