using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class FoundryRevenantBoilerplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaperClass) += 0.05f;
            player.GetCritChance(reaperClass) += 5f;
            player.statLifeMax2 += 25;

            // -10% soul ability cost
            player.GetModPlayer<ReaperAccessoryPlayer>().SoulCostMultiplier *= 0.90f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BrassBar>(), 20)
                .AddIngredient(ItemID.Cog, 50)
                .AddIngredient(ItemID.Wire, 50)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}