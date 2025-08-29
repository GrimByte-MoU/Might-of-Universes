using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using MightofUniverses.Content.Items.Materials;

namespace MightofUniverses.Common.GlobalNPCs
{
    public class PumpkinMoonDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.Pumpking)
            {
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<PureTerror>(), Main.rand.Next(3, 10));
            }

            if (npc.type == NPCID.MourningWood)
            {
                if (Main.rand.NextFloat() < .75f) // 75% chance
                {
                    Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<PureTerror>(), Main.rand.Next(2, 8));
                }
            }

            if (npc.type == NPCID.HeadlessHorseman)
            {
                if (Main.rand.NextFloat() < .50f) // 50% chance
                {
                    Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<PureTerror>(), Main.rand.Next(1, 4));
                }
            }
        }
    }
}
