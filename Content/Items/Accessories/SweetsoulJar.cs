using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class SweetsoulJar : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SweetsoulJarPlayer>().HasSweetsoulJar = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GummyMembrane>(), 8)
                .AddIngredient(ItemID.Bottle, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
    public class SweetsoulJarPlayer : ModPlayer
    {
        public bool HasSweetsoulJar;
        private int soulTickTimer;

        public override void ResetEffects()
        {
            HasSweetsoulJar = false;
        }

        public override void PostUpdate()
        {
            if (!HasSweetsoulJar)
            {
                soulTickTimer = 0;
                return;
            }

            soulTickTimer++;
            if (soulTickTimer >= 60)
            {
                soulTickTimer = 0;

                var reaper = Player.GetModPlayer<ReaperPlayer>();
                if (reaper.soulEnergy + 0.01f < reaper.maxSoulEnergy)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        reaper.AddSoulEnergy(1f, Player.Center);
                    }
                }
            }
        }
    }
}