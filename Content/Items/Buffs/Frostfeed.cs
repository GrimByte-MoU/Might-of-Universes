using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Buffs
{
    public class Frostfeed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }

        // Runs every tick on NPCs with this buff
        public override void Update(NPC npc, ref int buffIndex)
        {
            const int dps = 150;
            int regenLoss = dps * 2;

            if (npc.lifeRegen > 0)
                npc.lifeRegen = 0;

            npc.lifeRegen -= regenLoss;
            if (Main.rand.NextBool(10)) Dust.NewDust(npc.position, npc.width, npc.height, DustID.Ice);
        }
    }
}