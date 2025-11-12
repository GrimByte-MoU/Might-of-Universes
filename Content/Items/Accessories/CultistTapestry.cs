using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class CultistTapestry : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(gold: 15);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaperPlayer = player.GetModPlayer<ReaperPlayer>();
            reaperPlayer.hasCultistTapestry = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 3)
                .AddIngredient(ModContent.ItemType<LunaticCloth>(), 5)
                .AddIngredient(ItemID.AncientCloth, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}