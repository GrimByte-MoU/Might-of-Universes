using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class CyberOneLegArmor : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Purple;
            Item.defense = 30;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) *= 0.80f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.50f;
            player.moveSpeed += 0.30f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 10)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 10)
                .AddIngredient(ItemID.Nanites, 30)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}