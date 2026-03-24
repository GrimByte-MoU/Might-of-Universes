using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class PandorasBox : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.BonusEmpowerReaperDamage += 0.20f;
            acc.BonusEmpowerAttackSpeed += 0.10f;
            acc.BonusEmpowerCritChance += 12f;
            acc.BonusEmpowerArmorPen += 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<DybbukBox>()
                .AddIngredient(ItemID.LunarBar, 7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}