// Content/Items/Consumables/WorldsoulsMedallion.cs

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using MightofUniverses.Content.Rarities;

namespace MightofUniverses.Content.Items.Summoning
{
    public class WorldsoulsMedallion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 20;
            Item.rare = ModContent.RarityType<TerraiumRarity>();
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID. HoldUp; 
            Item.UseSound = SoundID. Roar;
            Item.consumable = false;
            Item. value = Item.sellPrice(gold: 5);
        }

        public override bool CanUseItem(Player player)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Aegon.Aegon>()))
            {
                return false;
            }
            return true;
        }

        public override bool?  UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int npcIndex = NPC.NewNPC(
                    new EntitySource_ItemUse(player, Item),
                    (int)player.Center.X,
                    (int)player.Center.Y,
                    ModContent.NPCType<NPCs. Bosses.Aegon. Aegon>()
                );

                if (npcIndex != Main.maxNPCs)
                {
                    NPC Aegon = Main.npc[npcIndex];

                    if (Main. netMode == NetmodeID. Server)
                    {
                        NetMessage.SendData(MessageID. SyncNPC, -1, -1, null, npcIndex);
                    }
                }
                CreateSummonEffect(player.Center);
            }

            return true;
        }
        private void CreateSummonEffect(Vector2 position)
        {
            float arenaRadius;
            if (Main.masterMode)
                arenaRadius = 75.5f;
            else if (Main.expertMode)
                arenaRadius = 87.5f;
            else
                arenaRadius = 100.5f;

            int segments = 200;
            for (int i = 0; i < segments; i++)
            {
                float angle = i / (float)segments * MathHelper.TwoPi;
                Vector2 dustPos = position + new Vector2(
                    (float)System.Math.Cos(angle) * arenaRadius * 16f,
                    (float)System.Math.Sin(angle) * arenaRadius * 16f
                );

                int dust = Dust.NewDust(dustPos, 0, 0, DustID. Stone, 0f, 0f, 100, Color.Brown, 2.5f);
                Main. dust[dust].noGravity = true;
                Main.dust[dust].velocity = Vector2.Zero;
            }

            for (int i = 0; i < 50; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(10f, 10f);
                int dust = Dust.NewDust(position, 0, 0, DustID.Stone, velocity.X, velocity.Y, 100, Color.Orange, 2f);
                Main.dust[dust].noGravity = true;
            }

            if (Main.LocalPlayer.Distance(position) < 2000f)
            {
                Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>()?. ScreenShake(15, 30);
            }
        }

        public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "SummonDesc", 
                "Summons Aegon, the World's Aegis"));
            
            tooltips.Add(new TooltipLine(Mod, "ArenaWarning", 
                "Creates an indestructible arena. Aegon is untouchable outside the arena")
            {
                OverrideColor = Color.Orange
            });

            tooltips.Add(new TooltipLine(Mod, "ArenaSize",
                $"Arena size: {GetArenaSize()} blocks")
            {
                OverrideColor = Color.Gray
            });
        }

        private string GetArenaSize()
        {
            if (Main.masterMode)
                return "151x151";
            else if (Main.expertMode)
                return "175x175";
            else
                return "201x201";
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ItemID.SoulofLight, 10)
                .AddIngredient(ItemID.SoulofNight, 10)
                .AddIngredient(ItemID.SoulofFlight, 5)
                .AddIngredient(ItemID.CrystalShard, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}