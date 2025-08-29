using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Buffs;

namespace MightofUniverses.Content.NPCs
{
    public class EmpressBuffs : GlobalNPC
    {
        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (npc.type == NPCID.HallowBoss)
            {
                target.AddBuff(ModContent.BuffType<PrismaticRend>(), 60);
                npc.velocity.X *= 0.8f;
                npc.velocity.Y *= 0.8f;
            }
        }
    }
}

