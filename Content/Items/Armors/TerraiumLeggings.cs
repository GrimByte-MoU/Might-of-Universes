using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Materials;
using System.Collections.Generic;

namespace MightofUniverses.Content.Items.Armors
{
    [AutoloadEquip(EquipType. Legs)]
    public class TerraiumLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            player.endurance += 0.05f;
            player.moveSpeed += 0.30f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TerraiumBar>(), 15)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}