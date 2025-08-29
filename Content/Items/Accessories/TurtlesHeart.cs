using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Accessories
{
    public class TurtlesHeart : ModItem
    {      public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<ReaperPlayer>();
            
            if (modPlayer.justConsumedSouls)
            {
                player.AddBuff(ModContent.BuffType<TurtlesVitality>(), 300); // 300 ticks = 5 seconds // 300 ticks = 5 seconds
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LifeFruit, 3)
                .AddIngredient(ItemID.TurtleShell, 3)
                .AddIngredient(ModContent.ItemType<JungleHeart>(), 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}