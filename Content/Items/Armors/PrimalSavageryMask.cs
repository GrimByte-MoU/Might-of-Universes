using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Head)]
    public class PrimalSavageryMask : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaper) += 0.08f;
            player.GetCritChance(reaper) += 7f;

            player.GetModPlayer<PrimalSavageryPlayer>().MaskEquipped = true;

            player.GetModPlayer<ReaperAccessoryPlayer>().SoulCostMultiplier *= 0.90f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PrimalSavageryChestplate>()
                && legs.type == ModContent.ItemType<PrimalSavageryBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus =
                "+350 max souls, +15% Reaper damage, +10% Reaper crit\n" +
                "Reaper attacks inflict Tarred and Oiled for 3 seconds \n" +
                "Consuming souls grants the Savage buff for 5 seconds";

            player.GetModPlayer<PrimalSavageryPlayer>().FullSetEquipped = true;
            player.GetModPlayer<ReaperPlayer>().hasReaperArmor = true;
            player.GetModPlayer<ReaperPlayer>().maxSoulEnergy += 350;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TarChunk>(), 6)
                .AddIngredient(ModContent.ItemType<PrehistoricAmber>(), 8)
                .AddIngredient(ModContent.ItemType<AncientBone>(), 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}