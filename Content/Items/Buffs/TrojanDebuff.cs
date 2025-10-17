using System;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace MightofUniverses.Content.Items.Buffs
{
    public class TrojanDebuff : ModBuff
    {
        private static Dictionary<int, int> npcDefenseStolen = new();

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public static void ApplyDebuff(NPC npc, int amount)
        {
            if (!npcDefenseStolen.ContainsKey(npc.whoAmI))
                npcDefenseStolen[npc.whoAmI] = 0;

            int actualSteal = Math.Min(amount, npc.defense);
            npc.defense -= actualSteal;
            npcDefenseStolen[npc.whoAmI] += actualSteal;

            npc.AddBuff(ModContent.BuffType<TrojanDebuff>(), 300);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.buffTime[buffIndex] == 1 && npcDefenseStolen.ContainsKey(npc.whoAmI))
            {
                npc.defense += npcDefenseStolen[npc.whoAmI];
                npcDefenseStolen.Remove(npc.whoAmI);
            }
        }
    }
}