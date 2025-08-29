using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class FrostLegionDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (!NPC.downedPlantBoss) return;

            if (npc.type == NPCID.MisterStabby || 
                npc.type == NPCID.SnowBalla || 
                npc.type == NPCID.SnowmanGangsta)
            {
                if (Main.rand.NextFloat() < .50f) // 30% chance
                {
                    Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<FrozenFragment>(), 1);
                }
            }
        }
    }
}
