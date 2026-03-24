using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using MightofUniverses.Common.Players;
using System.Collections.Generic;

namespace MightofUniverses.Content.Items.Buffs
{
    public class SoulEmpowerment : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            Player player = Main.LocalPlayer;
            var state = player.GetModPlayer<ReaperEmpowermentState>();

            if (state.Values != null)
            {
                List<string> bonuses = new List<string>();

                if (state.Values.ReaperDamage != 0f)
                    bonuses.Add($"+{state.Values.ReaperDamage * 100:F0}% reaper damage");

                if (state.Values.AttackSpeed != 0f)
                    bonuses.Add($"+{state.Values.AttackSpeed * 100:F0}% attack speed");

                if (state.Values.CritChance != 0f)
                    bonuses.Add($"+{state.Values.CritChance:F0}% critical strike chance");

                if (state.Values.Defense != 0)
                    bonuses.Add($"+{state.Values.Defense} defense");

                if (state.Values.Endurance != 0f)
                    bonuses.Add($"+{state.Values.Endurance * 100:F0}% damage reduction");

                if (state.Values.LifeRegen != 0)
                    bonuses.Add($"+{state.Values.LifeRegen / 2:F0} life/sec");

                if (state.Values.ArmorPenetration != 0)
                    bonuses.Add($"+{state.Values.ArmorPenetration} armor penetration");

                if (state.Values.LifestealPercent != 0)
                    bonuses.Add($"{state.Values.LifestealPercent}% lifesteal");

                if (state.Values.BonusMaxSouls != 0)
                    bonuses.Add($"+{state.Values.BonusMaxSouls} max souls");

                if (state.Values.SoulDrainPerSecond != 0)
                    bonuses.Add($"-{state.Values.SoulDrainPerSecond} souls/sec");

                if (state.Values.DamageTakenMultiplier != 0f)
                    bonuses.Add($"+{state.Values.DamageTakenMultiplier * 100:F0}% damage taken");

                if (bonuses.Count > 0)
                    tip = string.Join("\n", bonuses);
                else
                    tip = "Empowered by souls";
            }
            else
            {
                tip = "Empowered by souls";
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }
    }
}