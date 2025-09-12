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
            // "+15 max soul energy\nGenerates 1 soul per second while below 30% soul capacity."
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
            var acc = player.GetModPlayer<BoneTrinketPlayer>();
            acc.HasBoneTrinket = true;

            player.GetModPlayer<ReaperPlayer>().maxSoulEnergy += 15f;
        }

        public override void AddRecipes()
        {
            // Crafting route post-Skeletron
            CreateRecipe()
                .AddIngredient(ItemID.Bone, 20)
                .AddCondition(Condition.DownedSkeletron) // tModLoader condition (1.4+)
                .AddTile(TileID.BoneWelder, TileID.WorkBenches) // choose appropriate
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

            regenTimer++;
            if (regenTimer >= 60)
            {
                regenTimer = 0;
                var reaper = Player.GetModPlayer<ReaperPlayer>();

                if (reaper.soulEnergy < reaper.maxSoulEnergy * 0.30f - 0.01f)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        reaper.AddSoulEnergy(1f, Player.Center);
                        PassiveSoulRegistry.AddPassiveSoul(Player, 1f); // so Spirit String can convert
                    }
                }
            }
        }
    }
}