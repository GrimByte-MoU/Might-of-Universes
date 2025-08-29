using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Consumables.Food
{
    public class PiratesPorridge : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item2;
            Item.consumable = true;
            Item.rare = ItemRarityID.Yellow;
            Item.maxStack = 99;
            Item.buffType = ModContent.BuffType<PiratePower>();
            Item.buffTime = 1800; // 30 seconds
            Item.value = Item.sellPrice(silver: 50);
        }

        public override bool CanUseItem(Player player) => !player.HasBuff(Item.buffType);

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.GreedySpirit>(), 2)
                .AddTile(TileID.CookingPots)
                .Register();
        }
    }
}
