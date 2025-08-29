using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Buffs;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Consumables.Food
{
    public class BloodyBroth : ModItem
    {
        public override void SetDefaults()
        {
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item2;
            Item.consumable = true;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Yellow;
            Item.buffType = ModContent.BuffType<BrothVitality>();
            Item.buffTime = 1800; // 30 seconds
            Item.value = Item.sellPrice(silver: 50);
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(Item.buffType);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SanguineEssence>(), 2)
                .AddTile(TileID.CookingPots)
                .Register();
        }
    }
}
