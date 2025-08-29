// Location: Content/Items/Consumables/CandyBowl.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Materials;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Consumables.Food;

namespace MightofUniverses.Content.Items.Consumables.TreasureBags
{
    public class CandyBowl : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item4;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 50);
        }

        public override bool? UseItem(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                int rand = Main.rand.Next(4);

                switch (rand)
                {
                    case 0:
                        int gummyAmount = Main.rand.Next(1, 21);
                        player.QuickSpawnItem(null, ModContent.ItemType<GummyMembrane>(), gummyAmount);
                        break;
                    case 1:
                        int lollipopAmount = Main.rand.Next(1, 4);
                        player.QuickSpawnItem(null, ModContent.ItemType<RainbowLollipop>(), lollipopAmount);
                        break;
                    case 2:
                        int bubblegumAmount = Main.rand.Next(1, 4);
                        player.QuickSpawnItem(null, ModContent.ItemType<StickOfBubblegum>(), bubblegumAmount);
                        break;
                    case 3:
                        int taffyAmount = Main.rand.Next(1, 4);
                        player.QuickSpawnItem(null, ModContent.ItemType<ChewyTaffy>(), taffyAmount);
                        break;
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GummyMembrane>(), 10)
                .AddIngredient(ModContent.ItemType<SweetTooth>(), 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
