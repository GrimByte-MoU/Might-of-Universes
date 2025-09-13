using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Accessories
{
    public class IchorveinLocket : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 1, 10, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var p = player.GetModPlayer<ReaperAccessoryPlayer>();
            p.accIchorveinLocket = true;
            p.ApplyMaxSoulFromHP(0.15f);
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(Mod, "HematicLocket", 1);
            r.AddIngredient(ItemID.Ichor, 15);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();
        }
    }
}