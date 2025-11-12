using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class CharmOfMorbidFocus : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 10);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            int deathMarks = reaperPlayer.deathMarks;
            player.GetCritChance(DamageClass.Generic) += 3f * deathMarks;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 3)
                .AddIngredient(ItemID.RagePotion, 3)
                .AddIngredient(ItemID.AncientCloth, 2)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}