using Terraria;
using Terraria.ModLoader;

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
            if (Player.armor[0].type == ModContent.ItemType<Content.Items.Armors.BoneCollectorHat>()
             && Player.armor[1].type == ModContent.ItemType<Content.Items.Armors.BoneCollectorShirt>()
             && Player.armor[2].type == ModContent.ItemType<Content.Items.Armors.BoneCollectorPants>())
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
    }
}