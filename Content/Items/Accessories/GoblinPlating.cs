using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class GoblinPlating : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.defense = 3;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += (int)(player.statLifeMax * 0.10f);
            player.endurance += 0.05f;
            player.lifeRegen += 2;
            player.accDreamCatcher = true;
            player.thorns = 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoblinTool>())
                .AddIngredient(ItemID.RegenerationPotion, 5)
                .AddIngredient(ItemID.LifeforcePotion, 5)
                .AddIngredient(ItemID.EndurancePotion, 5)
                .AddIngredient(ItemID.IronskinPotion, 5)
                .AddIngredient(ItemID.ThornsPotion, 5)
                .AddIngredient(ItemID.DPSMeter)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
