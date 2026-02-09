using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.Items.Consumables.Food
{
    public class SolarSizzleSnack : ModItem
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
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item2;
            Item.consumable = true;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.buyPrice(silver: 60);
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<EclipseBlessing>(), 1800);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<EclipseLight>(), 2)
                .Register();
        }
    }
}
