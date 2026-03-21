using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Buffs
{
    public class Frostfeed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.lifeRegen > 0)
                npc.lifeRegen = 0;

            npc.lifeRegen -= 300;

            if (Main.rand.NextBool(10)) 
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Ice);
        }
    }
}