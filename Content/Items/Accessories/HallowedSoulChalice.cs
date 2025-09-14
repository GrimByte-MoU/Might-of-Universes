using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players; // Adjust if ReaperPlayer lives elsewhere

namespace MightofUniverses.Content.Items.Accessories
{
    public class HallowedSoulChalice : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<HallowedSoulChalicePlayer>().HasHallowedSoulChalice = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<InfernalSoulJar>(), 1)
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ItemID.SoulofLight, 10)
                .AddIngredient(ItemID.SoulofNight, 10)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class HallowedSoulChalicePlayer : ModPlayer
    {
        public bool HasHallowedSoulChalice;
        private int tickTimer;

        public override void ResetEffects()
        {
            HasHallowedSoulChalice = false;
        }

        public override void PostUpdate()
        {
            if (!HasHallowedSoulChalice)
            {
                tickTimer = 0;
                return;
            }

            tickTimer++;
            if (tickTimer >= 6) // 6 ticks ≈ 0.1 second
            {
                tickTimer = 0;

                var reaper = Player.GetModPlayer<ReaperPlayer>();
                if (reaper.soulEnergy + 0.01f < reaper.maxSoulEnergy)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        reaper.AddSoulEnergy(0.2f, Player.Center);
                    }
                }
            }
        }
    }
}