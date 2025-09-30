using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Common.Players
{
    public class BoneCollectorSetPlayer : ModPlayer
    {

        private bool wearingBoneCollectorSet;

        public override void ResetEffects()
        {
            wearingBoneCollectorSet = false;
        }

        public override void UpdateEquips()
        {
            if (IsWearingFullBoneCollectorSet())
            {
                wearingBoneCollectorSet = true;
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                reaper.hasReaperArmor = true;

                var acc = Player.GetModPlayer<ReaperAccessoryPlayer>();
                acc.flatMaxSoulsBonus += 100;

                if (acc.SoulCostMultiplier == 0f)
                    acc.SoulCostMultiplier = 1f;
                acc.SoulCostMultiplier *= 0.90f;

                acc.RefundChance += 0.02f;
            }
        }

        public override void PostUpdateEquips()
        {
            if (wearingBoneCollectorSet)
            {
                Player.setBonus = $"+{100} max souls\nSoul consumption costs 10% less.\nWhen you consume souls, there is a 2% chance to refund the entire cost.";
            }
        }

        private bool IsWearingFullBoneCollectorSet()
        {
            return Player.armor[0].type == ModContent.ItemType<Content.Items.Armors.BoneCollectorHat>()
                && Player.armor[1].type == ModContent.ItemType<Content.Items.Armors.BoneCollectorShirt>()
                && Player.armor[2].type == ModContent.ItemType<Content.Items.Armors.BoneCollectorPants>();
        }
    }
}