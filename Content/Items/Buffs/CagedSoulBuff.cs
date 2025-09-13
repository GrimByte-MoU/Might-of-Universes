using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Buffs
{
    // Player buff: +10% Reaper damage
    public class CagedSoulBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(ModContent.GetInstance<ReaperDamageClass>()) += 0.10f;
        }
    }
}