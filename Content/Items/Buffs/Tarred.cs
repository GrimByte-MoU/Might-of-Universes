using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Buffs
{
    public class Tarred : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.defense = (int)(npc.defense * 0.7f);
            if (npc.HasBuff(BuffID.OnFire3))
            {
                npc.lifeRegen -= 80;
            }
            if (npc.HasBuff(ModContent.BuffType<Demonfire>()))
            {
                npc.lifeRegen -= 150;
            }
        }
    }
}