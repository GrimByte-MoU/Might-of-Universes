using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class MechaCactusBreastplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 18;
            Item.defense = 19;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 8);
        }

        public override void UpdateEquip(Player player)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaperClass) += 0.05f;
            player.GetCritChance(reaperClass) += 5f;
            player.statLifeMax2 += 15;
            player.endurance += 0.02f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CactusBreastplate, 1)
                .AddIngredient(ModContent.ItemType<Kevlar>(), 15)
                .AddIngredient(ModContent.ItemType<CarbonFiber>(), 20)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}