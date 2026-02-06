using Terraria;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Common.NPCs
{
    public class SheerColdNPC : GlobalNPC
    {
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff<SheerCold>())
            {
                modifiers.Defense -= 25;
                modifiers.CritDamage += 0.5f;
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff<SheerCold>())
            {
                modifiers.Defense -= 25;
                modifiers.CritDamage += 0.5f;
            }
        }
    }
}