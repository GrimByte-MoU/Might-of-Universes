using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class PrimalSavageryBoots : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 9);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
            var reaper = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaper) += 0.08f;
            player.GetCritChance(reaper) += 7f;
            player.GetModPlayer<PrimalSavageryPlayer>().BootsEquipped = true;
            player.moveSpeed += 0.20f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TarChunk>(), 10)
                .AddIngredient(ModContent.ItemType<PrehistoricAmber>(), 12)
                .AddIngredient(ModContent.ItemType<AncientBone>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}