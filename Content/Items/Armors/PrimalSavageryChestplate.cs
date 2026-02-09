using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class PrimalSavageryChestplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 22;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaper) += 0.08f;
            player.GetCritChance(reaper) += 7f;
            player.GetModPlayer<PrimalSavageryPlayer>().ChestEquipped = true;
            player.GetModPlayer<PrimalSavageryPlayer>().ChestSoulGenActive = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TarChunk>(), 14)
                .AddIngredient(ModContent.ItemType<PrehistoricAmber>(), 20)
                .AddIngredient(ModContent.ItemType<AncientBone>(), 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}