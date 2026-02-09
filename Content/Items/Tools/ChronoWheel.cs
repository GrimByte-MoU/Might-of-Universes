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
            Item.consumable = false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "ChronoWheel1", 
                "Cycles through the times of day: Sunrise → Noon → Sunset → Midnight"));
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
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)2);
                packet.Send();
            }
            else
            {
                AdvanceToNextPhase();
            }

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
            const double SUNRISE = 0;
            const double NOON = 27000;
            const double SUNSET = 0;
            const double MIDNIGHT = 16200;

            if (Main.dayTime)
            {
                if (Main.time < 13500)
                {
                    Main.time = NOON;
                    Main.dayTime = true;
                    ShowTimeChangeMessage("Noon");
                }
                else
                {
                    Main.time = SUNSET;
                    Main.dayTime = false;
                    ShowTimeChangeMessage("Sunset");
                }
            }
            else
            {
                if (Main.time < 8100)
                {
                    Main.time = MIDNIGHT;
                    Main.dayTime = false;
                    ShowTimeChangeMessage("Midnight");
                }
                else
                {
                    Main.time = SUNRISE;
                    Main.dayTime = true;
                    ShowTimeChangeMessage("Sunrise");
                }
            }

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