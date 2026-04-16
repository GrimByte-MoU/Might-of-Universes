using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class HauntedPainting : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var acc = player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.BonusEmpowerDefense += 10;
            acc.BonusEmpowerEndurance += 0.08f;
            acc.BonusEmpowerLifeRegen += 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<StrangeSketch>()
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ModContent.ItemType<UnderworldMass>(), 5)
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddIngredient(ItemID.SoulofNight, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}