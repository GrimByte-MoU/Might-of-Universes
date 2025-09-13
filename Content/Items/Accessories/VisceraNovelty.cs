using MightofUniverses.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Accessories
{
    public class VisceraNovelty : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 26; Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.buyPrice(0, 5, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var p = player.GetModPlayer<ReaperAccessoryPlayer>();
            p.accVisceraNovelty = true;
            p.ApplyMaxSoulFromHP(0.20f);
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<IchorveinLocket>(), 1);
            r.AddIngredient(ModContent.ItemType<SkeletonKnickknack>(), 1);
            r.AddIngredient(ModContent.ItemType<SanguineEssence>(), 5);
            r.AddIngredient(ItemID.SoulofFright, 5);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();
        }
    }
}