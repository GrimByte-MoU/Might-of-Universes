using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class Backbone : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.endurance += 0.05f;
            player.lifeRegen += 1;
            player.statLifeMax2 += 10;
            player.GetModPlayer<BackbonePlayer>().hasBackbone = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TissueSample, 25)
                .AddIngredient(ItemID.Vertebrae, 10)
                .AddIngredient(ItemID.EndurancePotion, 3)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
