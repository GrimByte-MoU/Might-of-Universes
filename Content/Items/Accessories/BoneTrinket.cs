using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Accessories
{
    public class BoneTrinket : ModItem
    {
        public override void SetStaticDefaults()
        {
            // +15 max soul energy
            // Generates 1 soul per second while below 30% soul capacity.
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(silver: 60);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Prefer the shared extra-max-souls pathway so all max-soul math is centralized.
            player.GetModPlayer<ReaperAccessoryPlayer>().flatMaxSoulsBonus += 15;

            // If your system still reads max directly from ReaperPlayer, uncomment this line:
            // player.GetModPlayer<ReaperPlayer>().maxSoulEnergy += 15f;

            player.GetModPlayer<BoneTrinketPlayer>().HasBoneTrinket = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bone, 20)
                .AddCondition(Condition.DownedSkeletron)
                .AddTile(TileID.BoneWelder)
                .Register();
        }
    }

    public class BoneTrinketPlayer : ModPlayer
    {
        public bool HasBoneTrinket;
        private int regenTimer;

        public override void ResetEffects()
        {
            HasBoneTrinket = false;
        }

        public override void PostUpdate()
        {
            if (!HasBoneTrinket)
            {
                regenTimer = 0;
                return;
            }

            // 1 soul per second if below 30% capacity
            if (++regenTimer >= 60)
            {
                regenTimer = 0;

                var reaper = Player.GetModPlayer<ReaperPlayer>();
                float max = Math.Max(1f, reaper.maxSoulEnergy);
                if (reaper.soulEnergy <= max * 0.30f - 0.01f)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        // Actually add 1 soul (server-side)
                        reaper.AddSoulEnergy(1f, Player.Center);
                    }

                    // Report passive soul gain so Spirit String/Threads can convert it to life regen
                    ReaperAccessoryPlayer.ReportPassiveSoulGain(Player, 1f);
                }
            }
        }
    }
}