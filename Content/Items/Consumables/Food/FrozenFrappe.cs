using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Consumables.Food
{
    public class FrozenFrappe : ModItem
    {
        public override void SetDefaults()
        {
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item2;
            Item.consumable = true;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(silver: 25);
            Item.buffType = ModContent.BuffType<WinterGrace>();
            Item.buffTime = 1800;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.FrozenFragment>(), 2)
                .AddTile(TileID.CookingPots)
                .Register();
        }
    }
}
