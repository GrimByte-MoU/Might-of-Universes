using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class PirateDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (!NPC.downedPlantBoss) return;

            if (npc.type == NPCID.PirateDeckhand || 
                npc.type == NPCID.PirateCorsair || 
                npc.type == NPCID.PirateDeadeye)
            {
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<GreedySpirit>(), Main.rand.Next(1, 3));
            }
            
            if (npc.type == NPCID.PirateCaptain)
            {
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<GreedySpirit>(), Main.rand.Next(2, 5));
            }
            if (npc.type == NPCID.PirateShip)
            {
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<GreedySpirit>(), 20);
            }
        }
    }
}