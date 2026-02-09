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
            player.GetModPlayer<ReaperAccessoryPlayer>().flatMaxSoulsBonus += 15;
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
            if (++regenTimer >= 60)
            {
                regenTimer = 0;

                var reaper = Player.GetModPlayer<ReaperPlayer>();
                float max = Math.Max(1f, reaper.maxSoulEnergy);
                if (reaper.soulEnergy <= max * 0.30f - 0.01f)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        reaper.AddSoulEnergy(1f, Player.Center);
                    }
                    
                    ReaperAccessoryPlayer.ReportPassiveSoulGain(Player, 1f);
                }
            }
        }
    }
}