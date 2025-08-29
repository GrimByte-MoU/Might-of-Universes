using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;
using Terraria.GameContent.ItemDropRules;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class EmpressDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.HallowBoss)
            {
                // Now correctly ordered with minimum (1) less than maximum (3)
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HallowedLight>(), 1, 30, 51));
            }
        }
    }
}
