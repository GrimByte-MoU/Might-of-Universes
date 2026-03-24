using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using MightofUniverses.Content.Items.Accessories;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class OminousNoteDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.Ghost)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OminousNote>(), 10));
            }
        }
    }
}