using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common;
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

            var reaper = Player.GetModPlayer<ReaperPlayer>();
            if (reaper.justConsumedSouls)
            {
                Player.AddBuff(ModContent.BuffType<HolidayScream>(), 300);
            }

            Lighting.AddLight(Player.Center, 0.9f, 0.1f, 0.1f);

            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Confetti, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(4))
            {
                int dustType = Main.rand.NextBool() ? DustID.Snow : DustID.Blood;
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, dustType, 0f, 0f, 100, default, 0.8f);
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
            }

            if (Main.rand.NextBool(3))
            {
                Color color = Main.rand.NextBool() ? Color.Red : Color.White;
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Torch, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, color, 0.7f);
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
            }
        }

        public override void PostUpdateEquips()
        {
            if (!FullSetEquipped) return;
            var acc = Player.GetModPlayer<ReaperAccessoryPlayer>();
            acc.EmpowerCostMultiplier *= 0.85f;

            if (Player.HasBuff(ModContent.BuffType<HolidayScream>()))
            {
                Player.endurance += 0.12f;

                if (Player.lifeRegen > 0)
                {
                    int bonus = (int)Math.Round(Player.lifeRegen * 0.20f);
                    Player.lifeRegen += bonus;
                }
            }
        }

        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            if (Player.HasBuff(ModContent.BuffType<HolidayScream>()) && healValue > 0 && !quickHeal)
            {
                healValue = (int)Math.Ceiling(healValue * 1.25f);
            }
        }
    }
}