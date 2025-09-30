using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MightofUniverses.Common.Players;

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

        public override void Update(Player player, ref int buffIndex)
        {
            var state = player.GetModPlayer<ReaperEmpowermentState>().Values;

            if (state.ReaperDamage != 0f)
                player.GetModPlayer<ReaperPlayer>().reaperDamageMultiplier += state.ReaperDamage;

            if (state.AttackSpeed != 0f)
                player.GetAttackSpeed(DamageClass.Generic) += state.AttackSpeed;

            if (state.CritChance != 0f)
                player.GetCritChance(DamageClass.Generic) += state.CritChance;

            if (state.Endurance != 0f)
                player.endurance += state.Endurance;

            if (state.Defense != 0)
                player.statDefense += state.Defense;

            if (state.LifeRegen != 0)
                player.lifeRegen += state.LifeRegen;
        }
    }
}