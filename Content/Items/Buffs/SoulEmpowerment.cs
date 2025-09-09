using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common.Players;

namespace MightofUniverses.Content.Items.Buffs
{
    /// <summary>
    /// ModBuff for the centralized Soul Empowerment system.
    /// Simply indicates that empowerment is active - all logic is handled in ReaperEmpowermentState.
    /// </summary>
    public class SoulEmpowerment : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // All empowerment logic is handled in ReaperEmpowermentState
            // This buff just serves as a timer/indicator that empowerment is active
            var empowermentState = player.GetModPlayer<ReaperEmpowermentState>();
            
            // If somehow the empowerment state isn't active but buff is, clear it
            if (!empowermentState.IsEmpowered)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}