using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class BookEmpowermentPlayer : ModPlayer
    {
        public bool hasBook = false;
        public int bonusRefreshTicks = 0;
        public bool hasRefundChance = false;

        private float lastSoulCost = 0f;
        private bool wasEmpowered = false;

        public override void ResetEffects()
        {
            hasBook = false;
            bonusRefreshTicks = 0;
            hasRefundChance = false;
        }

        public override void PostUpdate()
        {
            var empowerState = Player.GetModPlayer<ReaperEmpowermentState>();
            bool currentlyEmpowered = empowerState.Empowered;

            if (!wasEmpowered && currentlyEmpowered)
            {
                var reaper = Player.GetModPlayer<ReaperPlayer>();
                if (reaper.justConsumedSouls && reaper.lastSoulsConsumed > 0f)
                {
                    lastSoulCost = reaper.lastSoulsConsumed;
                    Main.NewText($"Empowerment activated! Consumed {(int)lastSoulCost} souls", Color.Cyan);
                }
            }

            if (wasEmpowered && !currentlyEmpowered && hasRefundChance && lastSoulCost > 0f)
            {
                if (Main.rand.NextFloat() < 0.05f)
                {
                    var reaper = Player.GetModPlayer<ReaperPlayer>();
                    float refund = lastSoulCost * 0.5f;
                    reaper.AddSoulEnergy(refund, Player.Center);
                    
                    CombatText.NewText(Player.getRect(), Color.Gold, $"+{(int)refund} souls (refund)");
                }
                lastSoulCost = 0f;
            }

            wasEmpowered = currentlyEmpowered;
        }

        public void RecordSoulCost(float cost)
        {
            lastSoulCost = cost;
        }

        public int GetBonusRefreshTicks()
        {
            return hasBook ? bonusRefreshTicks : 0;
        }
    }
}