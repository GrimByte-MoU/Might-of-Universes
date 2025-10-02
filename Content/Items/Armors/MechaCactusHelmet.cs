using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class MechaCactusHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.defense = 13;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 6);
        }

        public override void UpdateEquip(Player player)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaperClass) += 0.04f;
            player.GetCritChance(reaperClass) += 4f;
            player.statLifeMax2 += 10;
            player.endurance += 0.01f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CactusHelmet, 1)
                .AddIngredient(ModContent.ItemType<Kevlar>(), 6)
                .AddIngredient(ModContent.ItemType<CarbonFiber>(), 8)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}