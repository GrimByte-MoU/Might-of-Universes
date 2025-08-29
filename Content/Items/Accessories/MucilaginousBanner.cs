
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Accessories
{
    public class MucilaginousBanner : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
            Item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.npcTypeNoAggro[NPCID.BlueSlime] = true;
            player.npcTypeNoAggro[NPCID.GreenSlime] = true;
            player.npcTypeNoAggro[NPCID.PurpleSlime] = true;
            player.npcTypeNoAggro[NPCID.RedSlime] = true;
            player.npcTypeNoAggro[NPCID.YellowSlime] = true;
            player.npcTypeNoAggro[NPCID.BlackSlime] = true;
            player.npcTypeNoAggro[NPCID.MotherSlime] = true;
            player.npcTypeNoAggro[NPCID.BabySlime] = true;
            player.npcTypeNoAggro[NPCID.Pinky] = true;
            
            player.endurance += 0.03f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.RoyalGel)
                .AddIngredient(ItemID.Gel, 100)
                .AddIngredient(ItemID.KingSlimeTrophy)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}