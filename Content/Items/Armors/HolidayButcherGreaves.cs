using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class HolidayButcherGreaves : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaper) += 0.06f;
            player.GetCritChance(reaper) += 6f;

            // +3% DR
            player.endurance += 0.03f;

            // +15% movement speed
            player.moveSpeed += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 8)
                .AddIngredient(ModContent.ItemType<FestiveSpirit>(), 8)
                .AddIngredient(ModContent.ItemType<PureTerror>(), 8)
                .AddIngredient(ItemID.Ectoplasm, 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}