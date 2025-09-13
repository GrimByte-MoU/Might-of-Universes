using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SpiritTalisman : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24; Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ReaperAccessoryPlayer>().accSpiritTalisman = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<ProtectionCharm>(), 1);
            r.AddIngredient(ItemID.SoulofNight, 5);
            r.AddIngredient(ItemID.SoulofMight, 5);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();
        }
    }
}