using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Consumables.Food
{
    public class MugOfFestivities : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.buyPrice(silver: 60);
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<FestiveComfort>(), 60 * 30);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.FestiveSpirit>(), 2)
                .Register();
        }
    }
}
