using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Accessories
{
    public class JungleHeart : ModItem
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
                player.AddBuff(ModContent.BuffType<JungleVitality>(), 300);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.JungleSpores, 10)
                .AddIngredient(ItemID.Vine, 2)
                .AddIngredient(ItemID.Stinger, 10)
                .AddIngredient(ItemID.LifeCrystal, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
