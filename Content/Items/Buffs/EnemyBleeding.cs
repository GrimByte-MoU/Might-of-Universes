using Terraria;
using Terraria.ModLoader;

namespace MightofUniverses.Content.Items.Buffs
{
    // Enemy debuff: applies a bleed DoT (about 20 DPS) while active.
    public class EnemyBleeding : ModBuff
    {

        // Apply the actual life loss here; no separate files needed.
        public override void Update(NPC npc, ref int buffIndex)
        {
            // Neutralize any positive regen first.
            if (npc.lifeRegen > 0) npc.lifeRegen = 0;

            // 40 lifeRegen = ~20 DPS because lifeRegen is half-HP/sec units.
            npc.lifeRegen -= 40;
        }
    }

    // Optional: ensure the combat text shows at least 20 DPS.
    // Kept in the same file to avoid bloat.
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