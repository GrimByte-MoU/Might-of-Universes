using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class FungalCollection : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 25;
            player.statDefense += 3;
           player.GetModPlayer<PacifistPlayer>().pacifistDamageMultiplier += 0.35f;

            // Enable mushroom spore effect
            player.GetModPlayer<FungalCollectionPlayer>().fungalCollection = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<RageFungi>())
                .AddIngredient(ModContent.ItemType<RotFungi>())
                .AddIngredient(ModContent.ItemType<MushroomSample>())
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
