using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MightofUniverses.Content.NPCs
{
    public class GolemBuffs : GlobalNPC
    {
        public override bool AppliesToEntity(NPC npc, bool lateInstantiation)
        {
            // Apply these modifications only to the Golem and its parts
            return npc.type == NPCID.Golem || npc.type == NPCID.GolemHead || npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight;
        }

        public override void SetDefaults(NPC npc)
        {
            if (AppliesToEntity(npc, false))
            {
                // Increase health by 30%
                npc.lifeMax = (int)(npc.lifeMax * 1.3);
            }
        }

        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.Golem)
            {
                // Increase horizontal movement speed by 30%
                npc.velocity.X *= 1.3f;

                // Double vertical movement speed
                npc.velocity.Y *= 2.0f;
            }
            else if (npc.type == NPCID.GolemHead || npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight)
            {
                // Optionally, apply the same speed changes to the Golem's parts
                npc.velocity.X *= 1.3f;
                npc.velocity.Y *= 2.0f;
            }
        }

        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (AppliesToEntity(npc, false))
            {
                // Increase damage dealt to the player by 25%
                modifiers.FinalDamage *= 1.25f;
            }
        }
    }
}

