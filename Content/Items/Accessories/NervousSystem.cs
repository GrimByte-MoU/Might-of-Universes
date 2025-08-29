using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class NervousSystem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.endurance += 0.07f;
              player.lifeRegen += 2;
            player.statLifeMax2 += 20;
            player.GetModPlayer<NervousSystemPlayer>().hasNervousSystem = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Backbone>())
                .AddIngredient(ItemID.BrainOfConfusion)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.Ichor, 5)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
