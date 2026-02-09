using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Consumables.Food
{
    public class HallowedPastry : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item2;
            Item.consumable = true;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 1);
            Item.buffType = ModContent.BuffType<CakeBlessing>();
            Item.buffTime = 1800;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HallowedLight>(), 2)
                .AddTile(TileID.CookingPots)
                .Register();
        }
    }
}
