using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Buffs
{
    public class ProtectionCantrip : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 3;
            player.lifeRegen += 4;
        }
    }
}