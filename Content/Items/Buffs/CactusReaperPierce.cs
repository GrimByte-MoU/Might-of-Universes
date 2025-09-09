using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common;

namespace MightofUniverses.Content.Items.Buffs
{
    // Temporary armor penetration for Reaper attacks
    public class CactusReaperPierce : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetArmorPenetration(ModContent.GetInstance<ReaperDamageClass>()) += 10;
        }
    }
}