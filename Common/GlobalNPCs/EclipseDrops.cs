using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class EclipseDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (NPC.downedPlantBoss) return;

            if (npc.type == NPCID.Mothron)
            {
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<EclipseLight>(), Main.rand.Next(3, 7));
            }

            if (npc.type == NPCID.Vampire)
            {
                if (Main.rand.NextFloat() < .50f)
                {
                    Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<EclipseLight>(), Main.rand.Next(1, 4));
                }
            }
        }
    }
}
