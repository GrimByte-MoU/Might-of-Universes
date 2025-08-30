using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using Terraria.Localization;

namespace MightofUniverses.Content.Items.Accessories
{
    public class OverclockingRemote : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MOUPlayer>().EquippedOverclockRemote = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wire, 24)
                .AddIngredient(ItemID.SilverBar, 5)
                .AddIngredient(ItemID.GoldBar, 2)
                .AddIngredient(ItemID.IronBar, 10)
                .AddIngredient(ItemID.CopperBar, 5)
                .AddTile(TileID.Anvils)
                .Register();

                CreateRecipe()
                .AddIngredient(ItemID.Wire, 24)
                .AddIngredient(ItemID. TungstenBar, 5)
                .AddIngredient(ItemID.PlatinumBar, 2)
                .AddIngredient(ItemID.LeadBar, 10)
                .AddIngredient(ItemID.TinBar, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}