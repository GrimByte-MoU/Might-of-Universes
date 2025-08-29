using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class GoblinDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.GoblinPeon || 
                npc.type == NPCID.GoblinThief || 
                npc.type == NPCID.GoblinWarrior || 
                npc.type == NPCID.GoblinArcher)
            {
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<GoblinScrap>(), Main.rand.Next(1, 3));
                if (Main.rand.NextFloat() < .25f)
                {
                    Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<GoblinLeather>());
                }
            }
            
            if (npc.type == NPCID.GoblinSorcerer)
            {
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<GoblinScrap>(), Main.rand.Next(2, 4));
                if (Main.rand.NextFloat() < .9f)
                {
                    Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<GoblinLeather>());
                }
            }
        }
    }
}
