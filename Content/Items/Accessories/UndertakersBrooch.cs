using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class UndertakersBrooch : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 2, silver: 50);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var reaper = player.GetModPlayer<ReaperPlayer>();
            reaper.maxSoulEnergy += 50f;
            reaper.reaperCritChance += 6f; // your internal crit system
            player.GetModPlayer<ReaperAccessoryPlayer>().HasUndertakersBrooch = true;
        }

        public override void AddRecipes()
        {
            // Graveyard recipe: require player to be in a Graveyard when crafting (tModLoader automatically enforces environment)
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 15)
                .AddIngredient(ItemID.Bone, 25)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}