using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using MightofUniverses.Common.Players; // Adjust if ReaperPlayer lives elsewhere

namespace MightofUniverses.Content.Items.Accessories
{
    public class InfernalSoulJar : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<InfernalSoulJarPlayer>().HasInfernalSoulJar = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SweetsoulJar>(), 1)
                .AddIngredient(ModContent.ItemType<DemonicEssence>(), 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class InfernalSoulJarPlayer : ModPlayer
    {
        public bool HasInfernalSoulJar;
        private int tickTimer;

        public override void ResetEffects()
        {
            HasInfernalSoulJar = false;
        }

        public override void PostUpdate()
        {
            if (!HasInfernalSoulJar)
            {
                tickTimer = 0;
                return;
            }

            tickTimer++;
            if (tickTimer >= 20) // 20 ticks ≈ 1/3 second
            {
                tickTimer = 0;

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