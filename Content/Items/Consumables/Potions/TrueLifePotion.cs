using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Rarities;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Consumables.Potions
{
    public class TrueLifePotion : ModItem
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
            Item.rare = ModContent.RarityType<GalaxyRarity>();
            Item.value = Item.buyPrice(gold: 10);
            Item.potion = true;
            Item.UseSound = SoundID.Item3;
        }

        public override bool? UseItem(Player player)
        {
            int healAmount = player.statLifeMax2 / 2;
            if (healAmount < 400)
                healAmount = 400;

            player.statLife += healAmount;
            player.HealEffect(healAmount, true);
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.statLife < player.statLifeMax2;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HealingElixir>(), 2)
                .AddIngredient (ModContent.ItemType<GalaxyBar>(), 1) 
                .AddTile(TileID.AlchemyTable)
                .Register();
        }
    }
}
