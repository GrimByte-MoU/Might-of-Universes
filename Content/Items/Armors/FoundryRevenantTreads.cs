using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class FoundryRevenantTreads : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 7);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            var reaperClass = ModContent.GetInstance<ReaperDamageClass>();
            player.GetDamage(reaperClass) += 0.05f;
            player.GetCritChance(reaperClass) += 5f;
            player.moveSpeed += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BrassBar>(), 18)
                .AddIngredient(ItemID.Cog, 30)
                .AddIngredient(ItemID.Wire, 30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}