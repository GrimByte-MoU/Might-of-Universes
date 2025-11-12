using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Content.Items.Tools
{
    public class ChronoWheel : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item4;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 5);
            Item.consumable = false; // Non-consumable!
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ChronoWheel1", 
                "Cycles through the times of day: Sunrise → Noon → Sunset → Midnight"));
            
            // Show current time
            string currentTime = GetCurrentTimePhase();
            tooltips.Add(new TooltipLine(Mod, "ChronoWheel2", 
                $"Current: {currentTime}")
            {
                OverrideColor = Color.Yellow
            });
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Client sends packet to server to change time
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)2); // Opcode 2 for time change
                packet.Send();
            }
            else
            {
                // Single player or server - change time directly
                AdvanceToNextPhase();
            }

            // Visual effects
            for (int i = 0; i < 30; i++)
            {
                Vector2 dustVel = Main.rand.NextVector2Circular(8f, 8f);
                int dustType = Main.dayTime ? DustID.SolarFlare : DustID.Shadowflame;
                int dust = Dust.NewDust(player.position, player.width, player.height, 
                    dustType, dustVel.X, dustVel.Y, 100, default, 2f);
                Main.dust[dust].noGravity = true;
            }

            return true;
        }

        private void AdvanceToNextPhase()
        {
            // Time constants (in Terraria ticks)
            const double SUNRISE = 0;        // 4:30 AM (start of day)
            const double NOON = 27000;       // 12:00 PM (midday)
            const double SUNSET = 0;         // 7:30 PM (start of night)
            const double MIDNIGHT = 16200;   // 12:00 AM (midnight in night cycle)

            // Determine what phase we're currently in and advance to next
            if (Main.dayTime)
            {
                // Currently daytime
                if (Main.time < 13500) // First half of day (before 10:15 AM)
                {
                    // Phase 1: Sunrise → Noon
                    Main.time = NOON;
                    Main.dayTime = true;
                    ShowTimeChangeMessage("Noon");
                }
                else
                {
                    // Phase 2: Noon → Sunset (switch to night)
                    Main.time = SUNSET;
                    Main.dayTime = false;
                    ShowTimeChangeMessage("Sunset");
                }
            }
            else
            {
                // Currently nighttime
                if (Main.time < 8100) // First half of night (before 10:15 PM)
                {
                    // Phase 3: Sunset → Midnight
                    Main.time = MIDNIGHT;
                    Main.dayTime = false;
                    ShowTimeChangeMessage("Midnight");
                }
                else
                {
                    // Phase 4: Midnight → Sunrise (switch to day)
                    Main.time = SUNRISE;
                    Main.dayTime = true;
                    ShowTimeChangeMessage("Sunrise");
                }
            }

            // Sync to clients if server
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }

        private void ShowTimeChangeMessage(string phase)
        {
            Color messageColor = phase switch
            {
                "Sunrise" => new Color(255, 200, 100),
                "Noon" => Color.Yellow,
                "Sunset" => new Color(255, 100, 50),
                "Midnight" => new Color(100, 100, 200),
                _ => Color.White
            };

            Main.NewText($"Time advanced to {phase}!", messageColor);
        }

        private string GetCurrentTimePhase()
        {
            if (Main.dayTime)
            {
                if (Main.time < 13500)
                    return "Morning/Sunrise";
                else
                    return "Afternoon/Noon";
            }
            else
            {
                if (Main.time < 8100)
                    return "Evening/Sunset";
                else
                    return "Night/Midnight";
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SolunarToken>(), 10)
                .AddIngredient(ItemID.PlatinumWatch, 1)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            // Alt recipe with Gold Watch
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SolunarToken>(), 10)
                .AddIngredient(ItemID.GoldWatch, 1)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}