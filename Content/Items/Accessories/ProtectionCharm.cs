using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class ProtectionCharm : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24; Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 1, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ReaperAccessoryPlayer>().accProtectionCharm = true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<TatteredCharm>(), 1);
            r.AddIngredient(ItemID.HellstoneBar, 5);
            r.AddIngredient(ModContent.ItemType<DemonicEssence>(), 5);
            r.AddTile(TileID.Anvils);
            r.Register();
        }
    }
}