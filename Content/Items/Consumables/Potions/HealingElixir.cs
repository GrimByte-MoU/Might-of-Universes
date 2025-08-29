using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Consumables.Potions
{
    public class HealingElixir : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 20;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.value = Item.buyPrice(gold: 5);
            Item.healLife = 300;
            Item.potion = true;
            Item.UseSound = SoundID.Item3;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SuperHealingPotion, 1)
                .AddIngredient(ModContent.ItemType<ElementalCore>(), 1)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
