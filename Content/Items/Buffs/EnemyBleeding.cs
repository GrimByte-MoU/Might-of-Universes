using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Buffs
{
    public class EnemyBleeding : ModBuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.lifeRegen > 0) npc.lifeRegen = 0;
            npc.lifeRegen -= 40;
        }
    }
    public class EnemyBleedingDebuffGlobalNPC : GlobalNPC
    {
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.HasBuff(ModContent.BuffType<EnemyBleeding>()))
            {
                if (damage < 20) damage = 20;
            }
        }
    }
}