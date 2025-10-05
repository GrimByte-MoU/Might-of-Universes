using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common; // ReaperDamageClass
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.Players
{
    public class HolidayButcherPlayer : ModPlayer
    {
        public bool FullSetEquipped;

        public override void ResetEffects()
        {
            FullSetEquipped = false;
        }

        public override void PostUpdate()
        {
            if (!FullSetEquipped) return;

            // When souls are consumed (any ability), apply Holiday Scream for 5s
            var reaper = Player.GetModPlayer<ReaperPlayer>();
            if (reaper.justConsumedSouls)
            {
                Player.AddBuff(ModContent.BuffType<HolidayScream>(), 300);
            }
        }

        public override void PostUpdateEquips()
        {
            if (!FullSetEquipped) return;
            var acc = Player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.EmpowerCostMultiplier *= 0.85f;

            if (Player.HasBuff(ModContent.BuffType<HolidayScream>()))
            {
                // +12% DR
                Player.endurance += 0.12f;

                // +20% positive life regen: if lifeRegen > 0, increase by +20%
                if (Player.lifeRegen > 0)
                {
                    int bonus = (int)Math.Round(Player.lifeRegen * 0.20f);
                    Player.lifeRegen += bonus;
                }
            }
        }

        // +25% healing from items (potions, etc.) while buff is active
        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            if (Player.HasBuff(ModContent.BuffType<HolidayScream>()) && healValue > 0 && !quickHeal)
            {
                healValue = (int)Math.Ceiling(healValue * 1.25f);
            }
        }
    }
}