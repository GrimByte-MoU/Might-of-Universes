using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.NPCs.Bosses;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Summoning
{
    public class HorrificTreat : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 20;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override bool CanUseItem(Player player)
        {
            // Only allow use if boss is not already alive
            return !NPC.AnyNPCs(ModContent.NPCType<ObeSadee>());
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<ObeSadee>());
                Main.NewText("Obe Sadee, the Clogged has awoken!", 150, 10, 20);
            }
            return true;
        }

         public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GummyMembrane>(), 20)
                .AddIngredient(ModContent.ItemType<SweetTooth>(), 15)
                .AddIngredient(ItemID.ShadowScale, 5)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}
