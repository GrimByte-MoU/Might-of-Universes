using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Content.Items.Armors;
using MightofUniverses.Common;
using MightofUniverses.Common.Players;
using MightofUniverses.Content.Items.Weapons;

namespace MightofUniverses.Common.Players
{
    public class BoneCollectorSetPlayer : ModPlayer
    {
        private const int MaxSoulBonus = 100;
        private bool wearing;

        public override void ResetEffects()
        {
            wearing = false;
        }

        public override void UpdateEquips()
        {
            if (Player.armor[0].type == ModContent.ItemType<BoneCollectorHat>()
             && Player.armor[1].type == ModContent.ItemType<BoneCollectorShirt>()
             && Player.armor[2].type == ModContent.ItemType<BoneCollectorPants>())
            {
                wearing = true;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;
                reaper.maxSoulEnergy += MaxSoulBonus;

                var acc = Player.GetModPlayer<ReaperAccessoryPlayer>();
                if (acc.SoulCostMultiplier == 0f)
                    acc.SoulCostMultiplier = 1f;
                acc.SoulCostMultiplier *= 0.90f;
                acc.RefundChance += 0.02f;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearing)
                Player.setBonus = "+100 max souls\nSoul consumption costs 10% less.\n2% chance to fully refund consumed souls.";
        }

        public override void PostUpdate()
        {
            if (!wearing) return;

            Lighting.AddLight(Player.Center, 0.2f, 0.5f, 0.8f);

            if (Main.rand.NextBool(3))
            {
                int dustType = Main.rand.NextBool(4) ? DustID.Bone : DustID.Water;
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, dustType, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }
        }
    }
}