using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Common.GlobalNPCs;
using Terraria.ID;

namespace MightofUniverses.Content.Items.Buffs
{
    public class MortalWound : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<MortalWoundNPC>().hasMortalWound = true;
            npc.lifeRegen -= 200;

            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, Scale: 1.3f);
            }
        }
    }
}