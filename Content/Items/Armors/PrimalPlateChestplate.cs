using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class PrimalPlateChestplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 12);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 30;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) *= 0.70f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.50f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AncientBone>(), 25)
                .AddIngredient(ModContent.ItemType<TarChunk>(), 20)
                .AddIngredient(ModContent.ItemType<PrehistoricAmber>(), 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}