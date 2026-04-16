using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class AntiqueHandMirror : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var mirror = player.GetModPlayer<MirrorReflectPlayer>();
            mirror.hasMirror = true;
            mirror.reflectPercent = 1.00f;
            mirror.bonusIFrames = 45;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<UnusualLookingGlass>()
                .AddIngredient(ItemID.CrossNecklace)
                .AddIngredient(ItemID.PocketMirror)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}