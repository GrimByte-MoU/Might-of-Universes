using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class CyberOneChestplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ItemRarityID.Purple;
            Item.defense = 40;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) *= 0.75f;
            player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.60f;
            player.statLifeMax2 += 125;
            player.noKnockback = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 12)
                .AddIngredient(ModContent.ItemType<BrassBar>(), 12)
                .AddIngredient(ItemID.Nanites, 50)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}