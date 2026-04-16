using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class MarysMirror : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var mirror = player.GetModPlayer<MirrorReflectPlayer>();
            mirror.hasMirror = true;
            mirror.reflectPercent = 1.25f;
            mirror.bonusIFrames = 60;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AntiqueHandMirror>()
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 5)
                .AddIngredient(ItemID.AshWood, 50)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}