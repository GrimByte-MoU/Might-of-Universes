using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.NPCs
{
    public class GolemBuffs : GlobalNPC
    {
        public override bool AppliesToEntity(NPC npc, bool lateInstantiation)
        {
            return npc.type == NPCID.Golem || npc.type == NPCID.GolemHead || npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight;
        }

        public override void SetDefaults(NPC npc)
        {
            if (AppliesToEntity(npc, false))
            {
                npc.lifeMax = (int)(npc.lifeMax * 1.3);
            }
        }

        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.Golem)
            {
                npc.velocity.X *= 1.3f;

                npc.velocity.Y *= 2.0f;
            }
            else if (npc.type == NPCID.GolemHead || npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight)
            {
                npc.velocity.X *= 1.3f;
                npc.velocity.Y *= 2.0f;
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (AppliesToEntity(npc, false))
            {
                modifiers.FinalDamage *= 1.25f;
            }
        }
    }
}

