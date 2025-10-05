using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class HolidayButcherHat : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 7);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaper) += 0.06f;
            player.GetCritChance(reaper) += 6f;
            player.GetModPlayer<ReaperAccessoryPlayer>().SoulCostMultiplier *= 0.95f;
            player.endurance += 0.03f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HolidayButcherChestplate>()
                && legs.type == ModContent.ItemType<HolidayButcherGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus =
                "+300 max souls\n" +
                "Consuming souls grants Holiday Scream buff for 5 seconds \n" +
                "Soul Empowerment abilities cost 15% less and last 2 seconds longer";
            player.GetModPlayer<HolidayButcherPlayer>().FullSetEquipped = true;
            player.GetModPlayer<ReaperPlayer>().hasReaperArmor = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 6)
                .AddIngredient(ModContent.ItemType<FestiveSpirit>(), 6)
                .AddIngredient(ModContent.ItemType<PureTerror>(), 6)
                .AddIngredient(ItemID.Ectoplasm, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}