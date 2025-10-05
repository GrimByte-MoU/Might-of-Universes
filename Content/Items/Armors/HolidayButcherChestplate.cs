using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class HolidayButcherChestplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 9);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaper) += 0.06f;
            player.GetCritChance(reaper) += 6f;
            player.statLifeMax2 += 50;
            player.endurance += 0.04f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 12)
                .AddIngredient(ModContent.ItemType<FestiveSpirit>(), 12)
                .AddIngredient(ModContent.ItemType<PureTerror>(), 12)
                .AddIngredient(ItemID.Ectoplasm, 24)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}