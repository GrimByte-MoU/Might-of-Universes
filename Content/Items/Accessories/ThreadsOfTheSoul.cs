using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Accessories
{
    public class ThreadsOfTheSoul : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var p = player.GetModPlayer<ReaperAccessoryPlayer>();
            p.accThreadsOfSoul = true;
            p.ApplyPassiveSoulToHPScalar(0.5f);
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<SpiritString>(), 5);
            r.AddIngredient(ItemID.SoulofLight, 10);
            r.AddTile(TileID.Loom);
            r.Register();
        }
    }
}